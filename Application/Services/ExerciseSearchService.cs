
// FitnessTracker.Exercises.Application/Services/ExerciseSearchService.cs
using FitnessTracker.Exercises.Core.Entities;
using FitnessTracker.Exercises.Core.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workout.Core.Interfaces.Repositories;
using Workout.Core.Interfaces.Services;

namespace Workout.Application.Services
{
    public class ExerciseSearchService : IExerciseSearchService
    {
        private readonly IExerciseRepository _exerciseRepository;
        private readonly ILogger<ExerciseSearchService> _logger;

        // In a real implementation, you might inject a cache or database repository to store search terms
        private static readonly List<string> _popularSearchTerms = new List<string>
        {
            "abs", "arms", "chest", "legs", "cardio", "strength", "beginner", "home", "no equipment"
        };

        public ExerciseSearchService(
            IExerciseRepository exerciseRepository,
            ILogger<ExerciseSearchService> logger)
        {
            _exerciseRepository = exerciseRepository ?? throw new ArgumentNullException(nameof(exerciseRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Exercise>> SearchExercisesAsync(string searchTerm, int? limit = null)
        {
            try
            {
                return await _exerciseRepository.SearchAsync(searchTerm, limit);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching exercises with term {SearchTerm}", searchTerm);
                throw;
            }
        }

        public async Task<IEnumerable<Exercise>> SearchAdvancedAsync(
            string searchTerm = null,
            string category = null,
            string muscleGroup = null,
            ExerciseDifficultyEnum? difficulty = null,
            string equipment = null)
        {
            try
            {
                return await _exerciseRepository.SearchAdvancedAsync(
                    searchTerm,
                    category,
                    muscleGroup,
                    difficulty,
                    equipment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error performing advanced search");
                throw;
            }
        }

        public async Task<IEnumerable<Exercise>> GetRecommendedExercisesAsync(int userId, int limit = 10)
        {
            try
            {
                // In a real implementation, this would use some algorithm to determine
                // recommended exercises based on user history, preferences, etc.

                // For this demo, we'll just return some random verified exercises
                var allExercises = await _exerciseRepository.GetAllAsync();
                var verifiedExercises = allExercises.Where(e => e.IsVerified).ToList();

                // Shuffle the list using a deterministic algorithm based on userId
                var random = new Random(userId);
                var shuffled = verifiedExercises.OrderBy(e => random.Next()).Take(limit).ToList();

                return shuffled;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting recommended exercises for user {UserId}", userId);
                throw;
            }
        }

        public Task<IEnumerable<string>> GetPopularSearchTermsAsync(int limit = 10)
        {
            // In a real implementation, this would retrieve actual popular search terms from a database
            return Task.FromResult<IEnumerable<string>>(_popularSearchTerms.Take(limit).ToList());
        }
    }
}
