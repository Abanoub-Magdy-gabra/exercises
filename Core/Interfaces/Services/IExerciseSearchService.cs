


// FitnessTracker.Exercises.Core/Interfaces/Services/IExerciseSearchService.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using FitnessTracker.Exercises.Core.Entities;
using FitnessTracker.Exercises.Core.Enums;

namespace Workout.Core.Interfaces.Services
{
    public interface IExerciseSearchService
    {
        Task<IEnumerable<Exercise>> SearchExercisesAsync(string searchTerm, int? limit = null);
        Task<IEnumerable<Exercise>> SearchAdvancedAsync(
            string searchTerm,
            string category = null,
            string muscleGroup = null,
            ExerciseDifficultyEnum? difficulty = null,
            string equipment = null);
        Task<IEnumerable<Exercise>> GetRecommendedExercisesAsync(int userId, int limit = 10);
        Task<IEnumerable<string>> GetPopularSearchTermsAsync(int limit = 10);
    }
}
