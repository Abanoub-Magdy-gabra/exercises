
// FitnessTracker.Exercises.Core/Interfaces/Repositories/IExerciseCategoryRepository.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using FitnessTracker.Exercises.Core.Entities;

namespace Workout.Core.Interfaces.Repositories
{
    public interface IExerciseCategoryRepository
    {
        Task<IEnumerable<ExerciseCategory>> GetAllAsync();
        Task<ExerciseCategory> GetByIdAsync(int categoryId);
        Task<IEnumerable<ExerciseCategory>> GetMainCategoriesAsync();
        Task<IEnumerable<ExerciseCategory>> GetSubcategoriesAsync(int parentCategoryId);
        Task<ExerciseCategory> CreateAsync(ExerciseCategory category);
        Task<ExerciseCategory> UpdateAsync(ExerciseCategory category);
        Task<bool> DeleteAsync(int categoryId);
        Task<bool> ExistsAsync(int categoryId);
        Task<bool> ExistsByNameAsync(string name);
    }
}
