using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Workout.Infrastructure.Stoarge
{
    public interface IMediaStorageService
    {
        Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType);
        Task<bool> DeleteFileAsync(string fileUrl);
        string GetFileUrl(string fileName);
        Task<Stream> DownloadFileAsync(string fileName);
    }

    public class MediaStorageService : IMediaStorageService
    {
        private readonly ILogger<MediaStorageService> _logger;
        private readonly string _storageType;
        private readonly string _localStoragePath;
        private readonly string _baseUrl;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;

        public MediaStorageService(IConfiguration configuration, ILogger<MediaStorageService> logger)
        {
            _logger = logger;
            _storageType = configuration["Storage:Type"]?.ToLower() ?? "local";
            _baseUrl = configuration["Storage:BaseUrl"] ?? "http://localhost:5000/api/exercises/media";

            if (_storageType == "azure")
            {
                var connectionString = configuration["Storage:Azure:ConnectionString"];
                _containerName = configuration["Storage:Azure:ContainerName"] ?? "exercise-media";

                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new ArgumentException("Azure Blob Storage connection string is not configured");
                }

                _blobServiceClient = new BlobServiceClient(connectionString);
            }
            else
            {
                // Default to local storage
                _localStoragePath = configuration["Storage:Local:Path"] ??
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Storage", "ExerciseMedia");

                // Create directory if it doesn't exist
                if (!Directory.Exists(_localStoragePath))
                {
                    Directory.CreateDirectory(_localStoragePath);
                }
            }

            _logger.LogInformation("Media storage service initialized with storage type: {StorageType}", _storageType);
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType)
        {
            try
            {
                var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(fileName)}";

                if (_storageType == "azure")
                {
                    return await UploadToAzureAsync(fileStream, uniqueFileName, contentType);
                }
                else
                {
                    return await UploadToLocalAsync(fileStream, uniqueFileName);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading file {FileName}", fileName);
                throw;
            }
        }

        public async Task<bool> DeleteFileAsync(string fileUrl)
        {
            try
            {
                var fileName = Path.GetFileName(fileUrl);

                if (_storageType == "azure")
                {
                    return await DeleteFromAzureAsync(fileName);
                }
                else
                {
                    return DeleteFromLocal(fileName);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting file {FileUrl}", fileUrl);
                return false;
            }
        }

        public string GetFileUrl(string fileName)
        {
            if (_storageType == "azure")
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
                var blobClient = containerClient.GetBlobClient(fileName);
                return blobClient.Uri.ToString();
            }
            else
            {
                return $"{_baseUrl}/{fileName}";
            }
        }

        public async Task<Stream> DownloadFileAsync(string fileName)
        {
            try
            {
                if (_storageType == "azure")
                {
                    return await DownloadFromAzureAsync(fileName);
                }
                else
                {
                    return DownloadFromLocal(fileName);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downloading file {FileName}", fileName);
                throw;
            }
        }

        #region Azure Blob Storage Implementation

        private async Task<string> UploadToAzureAsync(Stream fileStream, string fileName, string contentType)
        {
            // Get container client
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            // Get blob client
            var blobClient = containerClient.GetBlobClient(fileName);

            // Set content type
            var blobHttpHeaders = new BlobHttpHeaders
            {
                ContentType = contentType
            };

            // Upload file
            await blobClient.UploadAsync(fileStream, new BlobUploadOptions { HttpHeaders = blobHttpHeaders });

            // Return url
            return blobClient.Uri.ToString();
        }

        private async Task<bool> DeleteFromAzureAsync(string fileName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(fileName);

            return await blobClient.DeleteIfExistsAsync();
        }

        private async Task<Stream> DownloadFromAzureAsync(string fileName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(fileName);

            var downloadInfo = await blobClient.DownloadAsync();
            var memoryStream = new MemoryStream();
            await downloadInfo.Value.Content.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            return memoryStream;
        }

        #endregion

        #region Local Storage Implementation

        private async Task<string> UploadToLocalAsync(Stream fileStream, string fileName)
        {
            var filePath = Path.Combine(_localStoragePath, fileName);

            using (var fileStream2 = new FileStream(filePath, FileMode.Create))
            {
                await fileStream.CopyToAsync(fileStream2);
            }

            return $"{_baseUrl}/{fileName}";
        }

        private bool DeleteFromLocal(string fileName)
        {
            var filePath = Path.Combine(_localStoragePath, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }

            return false;
        }

        private Stream DownloadFromLocal(string fileName)
        {
            var filePath = Path.Combine(_localStoragePath, fileName);

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File not found: {fileName}");
            }

            var memoryStream = new MemoryStream();
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                fileStream.CopyTo(memoryStream);
            }

            memoryStream.Position = 0;
            return memoryStream;
        }

        #endregion
    }
}