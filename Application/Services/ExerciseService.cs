// FitnessTracker.Exercises.Application/Services/ExerciseService.cs
using Workout.Core.Constants;
using FitnessTracker.Exercises.Core.Entities;
using FitnessTracker.Exercises.Core.Enums;
using Workout.Core.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workout.Core.Interfaces.Repositories;

namespace Workout.Application.Services
{
    public class ExerciseService : IExerciseService
    {
        private readonly IExerciseRepository _exerciseRepository;
        private readonly ILogger<ExerciseService> _logger;

        public ExerciseService(
            IExerciseRepository exerciseRepository,
            ILogger<ExerciseService> logger)
        {
            _exerciseRepository = exerciseRepository ?? throw new ArgumentNullException(nameof(exerciseRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Exercise>> GetAllExercisesAsync()
        {
            try
            {
                return await _exerciseRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all exercises");
                throw;
            }
        }

        public async Task<Exercise> GetExerciseByIdAsync(int id)
        {
            try
            {
                return await _exerciseRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving exercise with ID {ExerciseId}", id);
                throw;
            }
        }

        public async Task<Exercise> GetExerciseDetailByIdAsync(int id)
        {
            try
            {
                return await _exerciseRepository.GetDetailByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving detailed exercise with ID {ExerciseId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<Exercise>> GetExercisesByCategoryAsync(string category)
        {
            try
            {
                return await _exerciseRepository.GetByCategoryAsync(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving exercises by category {Category}", category);
                throw;
            }
        }

        public async Task<IEnumerable<Exercise>> GetExercisesByMuscleGroupAsync(string muscleGroup)
        {
            try
            {
                return await _exerciseRepository.GetByMuscleGroupAsync(muscleGroup);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving exercises by muscle group {MuscleGroup}", muscleGroup);
                throw;
            }
        }

        public async Task<IEnumerable<Exercise>> GetExercisesByDifficultyAsync(ExerciseDifficultyEnum difficulty)
        {
            try
            {
                return await _exerciseRepository.GetByDifficultyAsync(difficulty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving exercises by difficulty {Difficulty}", difficulty);
                throw;
            }
        }

        public async Task<IEnumerable<Exercise>> GetExercisesByEquipmentAsync(string equipment)
        {
            try
            {
                return await _exerciseRepository.GetByEquipmentAsync(equipment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving exercises by equipment {Equipment}", equipment);
                throw;
            }
        }

        public async Task<Exercise> CreateExerciseAsync(Exercise exercise)
        {
            try
            {
                if (await _exerciseRepository.ExistsByNameAsync(exercise.Name))
                {
                    throw new InvalidOperationException($"An exercise with the name '{exercise.Name}' already exists.");
                }

                exercise.CreatedAt = DateTime.UtcNow;
                exercise.UpdatedAt = DateTime.UtcNow;
                exercise.IsVerified = false; // New exercises are not verified by default

                return await _exerciseRepository.CreateAsync(exercise);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating exercise {ExerciseName}", exercise.Name);
                throw;
            }
        }

        public async Task<Exercise> UpdateExerciseAsync(int id, Exercise exercise)
        {
            try
            {
                var existingExercise = await _exerciseRepository.GetByIdAsync(id);
                if (existingExercise == null)
                {
                    throw new KeyNotFoundException($"Exercise with ID {id} not found.");
                }

                // If the name is being changed, check if it will conflict with an existing exercise
                if (!string.IsNullOrEmpty(exercise.Name) &&
                    exercise.Name != existingExercise.Name &&
                    await _exerciseRepository.ExistsByNameAsync(exercise.Name))
                {
                    throw new InvalidOperationException($"An exercise with the name '{exercise.Name}' already exists.");
                }

                exercise.ExerciseId = id;
                exercise.UpdatedAt = DateTime.UtcNow;
                exercise.CreatedAt = existingExercise.CreatedAt;
                exercise.CreatedBy = existingExercise.CreatedBy;

                return await _exerciseRepository.UpdateAsync(exercise);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating exercise with ID {ExerciseId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteExerciseAsync(int id)
        {
            try
            {
                var exercise = await _exerciseRepository.GetByIdAsync(id);
                if (exercise == null)
                {
                    return false;
                }

                return await _exerciseRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting exercise with ID {ExerciseId}", id);
                throw;
            }
        }

        public async Task<bool> VerifyExerciseAsync(int id)
        {
            try
            {
                var exercise = await _exerciseRepository.GetByIdAsync(id);
                if (exercise == null)
                {
                    return false;
                }

                exercise.IsVerified = true;
                exercise.UpdatedAt = DateTime.UtcNow;

                await _exerciseRepository.UpdateAsync(exercise);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying exercise with ID {ExerciseId}", id);
                throw;
            }
        }
    }
}