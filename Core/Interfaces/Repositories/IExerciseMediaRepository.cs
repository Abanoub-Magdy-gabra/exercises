
// FitnessTracker.Exercises.Core/Interfaces/Repositories/IExerciseMediaRepository.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using FitnessTracker.Exercises.Core.Entities;
using FitnessTracker.Exercises.Core.Enums;

namespace Workout.Core.Interfaces.Repositories
{
    public interface IExerciseMediaRepository
    {
        Task<IEnumerable<ExerciseMedia>> GetByExerciseIdAsync(int exerciseId);
        Task<ExerciseMedia> GetByIdAsync(int mediaId);
        Task<IEnumerable<ExerciseMedia>> GetByExerciseIdAndTypeAsync(int exerciseId, MediaTypeEnum mediaType);
        Task<ExerciseMedia> CreateAsync(ExerciseMedia media);
        Task<ExerciseMedia> UpdateAsync(ExerciseMedia media);
        Task<bool> DeleteAsync(int mediaId);
        Task<bool> ExistsAsync(int mediaId);
    }
}