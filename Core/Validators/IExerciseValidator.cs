using FitnessTracker.Exercises.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Workout.Core.Validators
{
    /// <summary>
    /// Interface for validating Exercise entities
    /// </summary>
    public interface IExerciseValidator
    {
        /// <summary>
        /// Validates an exercise entity for creation
        /// </summary>
        /// <param name="exercise">The exercise to validate</param>
        /// <returns>A list of validation errors, empty if valid</returns>
        Task<IList<string>> ValidateForCreationAsync(Exercise exercise);

        /// <summary>
        /// Validates an exercise entity for update
        /// </summary>
        /// <param name="exercise">The exercise to validate</param>
        /// <returns>A list of validation errors, empty if valid</returns>
        Task<IList<string>> ValidateForUpdateAsync(Exercise exercise);

        /// <summary>
        /// Validates if an exercise name is unique
        /// </summary>
        /// <param name="name">The name to check</param>
        /// <param name="excludeExerciseId">Optional exercise ID to exclude from the check (used for updates)</param>
        /// <returns>True if the name is unique, false otherwise</returns>
        Task<bool> IsNameUniqueAsync(string name, int? excludeExerciseId = null);

        /// <summary>
        /// Validates if all the muscle groups in the exercise exist
        /// </summary>
        /// <param name="muscleGroups">List of muscle group names</param>
        /// <returns>True if all muscle groups exist, false otherwise</returns>
        Task<bool> AreMuscleGroupsValidAsync(IEnumerable<string> muscleGroups);

        /// <summary>
        /// Validates if the category name is valid
        /// </summary>
        /// <param name="categoryName">The category name to validate</param>
        /// <returns>True if the category name is valid, false otherwise</returns>
        Task<bool> IsCategoryValidAsync(string categoryName);
    }
}