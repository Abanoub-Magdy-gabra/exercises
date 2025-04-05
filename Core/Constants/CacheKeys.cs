// FitnessTracker.Exercises.Core/Constants/CacheKeys.cs
namespace Workout.Core.Constants
{
    public static class CacheKeys
    {
        public const string AllExercises = "exercises_all";
        public const string ExerciseById = "exercise_{0}"; // {0} = exerciseId
        public const string ExercisesByCategory = "exercises_category_{0}"; // {0} = category
        public const string ExercisesByMuscleGroup = "exercises_muscle_{0}"; // {0} = muscleGroup
        public const string ExercisesByDifficulty = "exercises_difficulty_{0}"; // {0} = difficulty
        public const string AllMuscleGroups = "muscle_groups_all";
        public const string AllCategories = "categories_all";
    }
}