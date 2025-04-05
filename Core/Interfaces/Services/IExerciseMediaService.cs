
// FitnessTracker.Exercises.Core/Interfaces/Services/IExerciseMediaService.cs
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FitnessTracker.Exercises.Core.Entities;
using FitnessTracker.Exercises.Core.Enums;

namespace Workout.Core.Interfaces.Services
{
    public interface IExerciseMediaService
    {
        Task<IEnumerable<ExerciseMedia>> GetMediaByExerciseIdAsync(int exerciseId);
        Task<ExerciseMedia> GetMediaByIdAsync(int mediaId);
        Task<ExerciseMedia> AddMediaAsync(int exerciseId, Stream fileStream, string fileName, string contentType, MediaTypeEnum mediaType);
        Task<ExerciseMedia> UpdateMediaAsync(int mediaId, ExerciseMedia media);
        Task<bool> DeleteMediaAsync(int mediaId);
        Task<string> GenerateThumbnailAsync(Stream fileStream, string contentType);
    }
}
