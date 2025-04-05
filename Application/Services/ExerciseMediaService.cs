

// FitnessTracker.Exercises.Application/Services/ExerciseMediaService.cs
using FitnessTracker.Exercises.Core.Constants;
using FitnessTracker.Exercises.Core.Entities;
using FitnessTracker.Exercises.Core.Enums;
using Workout.Core.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Workout.Core.Interfaces.Repositories;


namespace Workout.Application.Services
{
    public class ExerciseMediaService : IExerciseMediaService
    {
        private readonly IExerciseMediaRepository _mediaRepository;
        private readonly IExerciseRepository _exerciseRepository;
        private readonly ILogger<ExerciseMediaService> _logger;
        private readonly string _storageBasePath;
        private readonly string[] _allowedImageTypes = { "image/jpeg", "image/png", "image/gif" };
        private readonly string[] _allowedVideoTypes = { "video/mp4", "video/webm" };
        private readonly long _maxFileSize = 10 * 1024 * 1024; // 10MB

        public ExerciseMediaService(
            IExerciseMediaRepository mediaRepository,
            IExerciseRepository exerciseRepository,
            ILogger<ExerciseMediaService> logger)
        {
            _mediaRepository = mediaRepository ?? throw new ArgumentNullException(nameof(mediaRepository));
            _exerciseRepository = exerciseRepository ?? throw new ArgumentNullException(nameof(exerciseRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            // In a real implementation, this would be a configuration value
            _storageBasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Storage", "ExerciseMedia");

            // Create storage directory if it doesn't exist
            if (!Directory.Exists(_storageBasePath))
            {
                Directory.CreateDirectory(_storageBasePath);
            }
        }

        public async Task<IEnumerable<ExerciseMedia>> GetMediaByExerciseIdAsync(int exerciseId)
        {
            try
            {
                return await _mediaRepository.GetByExerciseIdAsync(exerciseId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving media for exercise ID {ExerciseId}", exerciseId);
                throw;
            }
        }

        public async Task<ExerciseMedia> GetMediaByIdAsync(int mediaId)
        {
            try
            {
                return await _mediaRepository.GetByIdAsync(mediaId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving media with ID {MediaId}", mediaId);
                throw;
            }
        }

        public async Task<ExerciseMedia> AddMediaAsync(int exerciseId, Stream fileStream, string fileName, string contentType, MediaTypeEnum mediaType)
        {
            try
            {
                // Validate exercise exists
                if (!await _exerciseRepository.ExistsAsync(exerciseId))
                {
                    throw new KeyNotFoundException($"Exercise with ID {exerciseId} not found");
                }

                // Validate file type
                ValidateFileType(contentType, mediaType);

                // Validate file size
                if (fileStream.Length > _maxFileSize)
                {
                    throw new ArgumentException(ErrorMessages.MaxFileSizeExceeded);
                }

                // Generate unique filename
                var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(fileName)}";
                var filePath = Path.Combine(_storageBasePath, uniqueFileName);

                // Save file
                using (var fileStream2 = new FileStream(filePath, FileMode.Create))
                {
                    await fileStream.CopyToAsync(fileStream2);
                }

                // Create thumbnail for images
                string thumbnailUrl = null;
                if (mediaType == MediaTypeEnum.Image)
                {
                    thumbnailUrl = await GenerateThumbnailAsync(fileStream, contentType);
                }

                // Create media record
                var media = new ExerciseMedia
                {
                    ExerciseId = exerciseId,
                    MediaType = mediaType,
                    Url = $"/api/exercise-media/{uniqueFileName}",
                    ThumbnailUrl = thumbnailUrl,
                    FileName = fileName,
                    FileSize = fileStream.Length,
                    ContentType = contentType,
                    CreatedAt = DateTime.UtcNow
                };

                return await _mediaRepository.CreateAsync(media);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding media for exercise ID {ExerciseId}", exerciseId);
                throw;
            }
        }

        public async Task<ExerciseMedia> UpdateMediaAsync(int mediaId, ExerciseMedia media)
        {
            try
            {
                var existingMedia = await _mediaRepository.GetByIdAsync(mediaId);
                if (existingMedia == null)
                {
                    throw new KeyNotFoundException($"Media with ID {mediaId} not found");
                }

                // Only update description or other metadata, not the actual file
                media.MediaId = mediaId;

                return await _mediaRepository.UpdateAsync(media);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating media with ID {MediaId}", mediaId);
                throw;
            }
        }

        public async Task<bool> DeleteMediaAsync(int mediaId)
        {
            try
            {
                var media = await _mediaRepository.GetByIdAsync(mediaId);
                if (media == null)
                {
                    return false;
                }

                // Delete the physical file
                var fileName = Path.GetFileName(media.Url);
                var filePath = Path.Combine(_storageBasePath, fileName);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                // Delete thumbnail if exists
                if (!string.IsNullOrEmpty(media.ThumbnailUrl))
                {
                    var thumbnailFileName = Path.GetFileName(media.ThumbnailUrl);
                    var thumbnailPath = Path.Combine(_storageBasePath, thumbnailFileName);

                    if (File.Exists(thumbnailPath))
                    {
                        File.Delete(thumbnailPath);
                    }
                }

                // Delete from database
                return await _mediaRepository.DeleteAsync(mediaId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting media with ID {MediaId}", mediaId);
                throw;
            }
        }

        public async Task<string> GenerateThumbnailAsync(Stream fileStream, string contentType)
        {
            // In a real implementation, this would use something like ImageSharp or System.Drawing
            // to resize the image and create a thumbnail

            // For this example, we'll just create a simple placeholder implementation
            try
            {
                // Reset stream position
                fileStream.Position = 0;

                var thumbnailFileName = $"thumb_{Guid.NewGuid()}.jpg";
                var thumbnailPath = Path.Combine(_storageBasePath, thumbnailFileName);

                // In a real implementation, we would resize the image here
                // For this demo, we're just copying the original file
                using (var thumbnailStream = new FileStream(thumbnailPath, FileMode.Create))
                {
                    await fileStream.CopyToAsync(thumbnailStream);
                }

                // Reset stream position again for potential further use
                fileStream.Position = 0;

                return $"/api/exercise-media/thumbnails/{thumbnailFileName}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating thumbnail");
                return null;
            }
        }

        private void ValidateFileType(string contentType, MediaTypeEnum mediaType)
        {
            switch (mediaType)
            {
                case MediaTypeEnum.Image:
                    if (!_allowedImageTypes.Contains(contentType.ToLower()))
                    {
                        throw new ArgumentException(ErrorMessages.InvalidFileType);
                    }
                    break;
                case MediaTypeEnum.Video:
                    if (!_allowedVideoTypes.Contains(contentType.ToLower()))
                    {
                        throw new ArgumentException(ErrorMessages.InvalidFileType);
                    }
                    break;
                case MediaTypeEnum.GIF:
                    if (contentType.ToLower() != "image/gif")
                    {
                        throw new ArgumentException(ErrorMessages.InvalidFileType);
                    }
                    break;
                default:
                    throw new ArgumentException(ErrorMessages.InvalidMediaType);
            }
        }
    }
}
