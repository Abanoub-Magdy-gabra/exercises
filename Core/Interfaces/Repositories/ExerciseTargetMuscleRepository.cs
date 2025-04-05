
// FitnessTracker.Exercises.Core/Interfaces/Repositories/IExerciseTargetMuscleRepository.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using FitnessTracker.Exercises.Core.Entities;

namespace Workout.Core.Interfaces.Repositories
{
    public interface IExerciseTargetMuscleRepository
    {
        Task<IEnumerable<ExerciseTargetMuscle>> GetByExerciseIdAsync(int exerciseId);
        Task<IEnumerable<ExerciseTargetMuscle>> GetByMuscleGroupAsync(string muscleGroup);
        Task<ExerciseTargetMuscle> AddAsync(ExerciseTargetMuscle targetMuscle);
        Task<bool> RemoveAsync(int exerciseId, string muscleGroup);
        Task<bool> UpdatePrimaryStatusAsync(int exerciseId, string muscleGroup, bool isPrimary);
        Task<bool> ExistsAsync(int exerciseId, string muscleGroup);
    }
}