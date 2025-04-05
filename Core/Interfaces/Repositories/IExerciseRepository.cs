// FitnessTracker.Exercises.Core/Interfaces/Repositories/IExerciseRepository.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using FitnessTracker.Exercises.Core.Entities;
using FitnessTracker.Exercises.Core.Enums;

namespace Workout.Core.Interfaces.Repositories
{
    public interface IExerciseRepository
    {
        Task<IEnumerable<Exercise>> GetAllAsync();
        Task<Exercise> GetByIdAsync(int id);
        Task<Exercise> GetDetailByIdAsync(int id);
        Task<IEnumerable<Exercise>> GetByCategoryAsync(string category);
        Task<IEnumerable<Exercise>> GetByMuscleGroupAsync(string muscleGroup);
        Task<IEnumerable<Exercise>> GetByDifficultyAsync(ExerciseDifficultyEnum difficulty);
        Task<IEnumerable<Exercise>> GetByEquipmentAsync(string equipment);
        Task<IEnumerable<Exercise>> SearchAsync(string searchTerm, int? limit = null);
        Task<IEnumerable<Exercise>> SearchAdvancedAsync(string searchTerm, string category, string muscleGroup, ExerciseDifficultyEnum? difficulty, string equipment);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsByNameAsync(string name);
        Task<Exercise> CreateAsync(Exercise exercise);
        Task<Exercise> UpdateAsync(Exercise exercise);
        Task<bool> DeleteAsync(int id);
        Task<int> GetTotalCountAsync();
    }
}
