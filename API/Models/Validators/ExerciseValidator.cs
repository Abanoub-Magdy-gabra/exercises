using FitnessTracker.Exercises.Core.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workout.Core.Interfaces.Repositories;
using Workout.Core.Validators;

namespace FitnessTracker.Exercises.Application.Validators
{
    public class ExerciseValidator : IExerciseValidator
    {
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IMuscleGroupRepository _muscleGroupRepository;
        private readonly IExerciseCategoryRepository _categoryRepository;

        public ExerciseValidator(
            IExerciseRepository exerciseRepository,
            IMuscleGroupRepository muscleGroupRepository,
            IExerciseCategoryRepository categoryRepository)
        {
            _exerciseRepository = exerciseRepository ?? throw new ArgumentNullException(nameof(exerciseRepository));
            _muscleGroupRepository = muscleGroupRepository ?? throw new ArgumentNullException(nameof(muscleGroupRepository));
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }

        public async Task<IList<string>> ValidateForCreationAsync(Exercise exercise)
        {
            var errors = new List<string>();

            // Basic validation
            if (exercise == null)
            {
                errors.Add("Exercise cannot be null");
                return errors;
            }

            // Name validation
            if (string.IsNullOrWhiteSpace(exercise.Name))
            {
                errors.Add("Exercise name is required");
            }
            else if (exercise.Name.Length < 3 || exercise.Name.Length > 100)
            {
                errors.Add("Exercise name must be between 3 and 100 characters");
            }
            else if (!await IsNameUniqueAsync(exercise.Name))
            {
                errors.Add($"An exercise with the name '{exercise.Name}' already exists");
            }

            // Category validation
            if (string.IsNullOrWhiteSpace(exercise.Category))
            {
                errors.Add("Category is required");
            }
            else if (!await IsCategoryValidAsync(exercise.Category))
            {
                errors.Add($"Category '{exercise.Category}' is not valid");
            }

            // Muscle groups validation
            if (exercise.TargetMuscles != null && exercise.TargetMuscles.Any())
            {
                var muscleGroups = exercise.TargetMuscles.Select(tm => tm.MuscleGroup);
                if (!await AreMuscleGroupsValidAsync(muscleGroups))
                {
                    errors.Add("One or more muscle groups are invalid");
                }

                // Check if there's at least one primary muscle group
                if (!exercise.TargetMuscles.Any(tm => tm.IsPrimary))
                {
                    errors.Add("At least one primary muscle group is required");
                }
            }
            else
            {
                errors.Add("At least one target muscle group is required");
            }

            return errors;
        }

        public async Task<IList<string>> ValidateForUpdateAsync(Exercise exercise)
        {
            var errors = new List<string>();

            // Basic validation
            if (exercise == null)
            {
                errors.Add("Exercise cannot be null");
                return errors;
            }

            // Name validation
            if (!string.IsNullOrWhiteSpace(exercise.Name))
            {
                if (exercise.Name.Length < 3 || exercise.Name.Length > 100)
                {
                    errors.Add("Exercise name must be between 3 and 100 characters");
                }
                else if (!await IsNameUniqueAsync(exercise.Name, exercise.ExerciseId))
                {
                    errors.Add($"An exercise with the name '{exercise.Name}' already exists");
                }
            }

            // Category validation
            if (!string.IsNullOrWhiteSpace(exercise.Category) && !await IsCategoryValidAsync(exercise.Category))
            {
                errors.Add($"Category '{exercise.Category}' is not valid");
            }

            return errors;
        }

        public async Task<bool> IsNameUniqueAsync(string name, int? excludeExerciseId = null)
        {
            // Check if any exercise with the same name already exists
            var existingExercises = await _exerciseRepository.SearchAsync(name);

            // If no exercises found with this name, or the only one is the excluded one, the name is unique
            return !existingExercises.Any(e =>
                e.Name.Equals(name, StringComparison.OrdinalIgnoreCase) &&
                (!excludeExerciseId.HasValue || e.ExerciseId != excludeExerciseId));
        }

        public async Task<bool> AreMuscleGroupsValidAsync(IEnumerable<string> muscleGroups)
        {
            if (muscleGroups == null || !muscleGroups.Any())
                return false;

            var allMuscleGroups = await _muscleGroupRepository.GetAllAsync();
            var validMuscleGroupNames = allMuscleGroups.Select(m => m.Name).ToList();

            // Check if all provided muscle groups exist in the valid list
            return muscleGroups.All(mg => validMuscleGroupNames.Contains(mg, StringComparer.OrdinalIgnoreCase));
        }

        public async Task<bool> IsCategoryValidAsync(string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
                return false;

            var allCategories = await _categoryRepository.GetAllAsync();
            return allCategories.Any(c => c.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase));
        }
    }
}