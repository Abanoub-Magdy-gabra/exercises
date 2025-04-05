
// FitnessTracker.Exercises.Application/Helpers/ExerciseFilterHelper.cs
using FitnessTracker.Exercises.Core.Entities;
using FitnessTracker.Exercises.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FitnessTracker.Exercises.Application.Helpers
{
    public static class ExerciseFilterHelper
    {
        public static IEnumerable<Exercise> FilterByDifficulty(this IEnumerable<Exercise> exercises, ExerciseDifficultyEnum difficulty)
        {
            return exercises.Where(e => e.Difficulty == difficulty);
        }

        public static IEnumerable<Exercise> FilterByCategory(this IEnumerable<Exercise> exercises, string category)
        {
            if (string.IsNullOrWhiteSpace(category))
                return exercises;

            return exercises.Where(e => e.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
        }

        public static IEnumerable<Exercise> FilterByEquipment(this IEnumerable<Exercise> exercises, string equipment)
        {
            if (string.IsNullOrWhiteSpace(equipment))
                return exercises;

            return exercises.Where(e => e.Equipment.Equals(equipment, StringComparison.OrdinalIgnoreCase));
        }

        public static IEnumerable<Exercise> FilterByMuscleGroup(this IEnumerable<Exercise> exercises, string muscleGroup)
        {
            if (string.IsNullOrWhiteSpace(muscleGroup))
                return exercises;

            return exercises.Where(e =>
                e.TargetMuscles != null &&
                e.TargetMuscles.Any(tm => tm.MuscleGroup.Equals(muscleGroup, StringComparison.OrdinalIgnoreCase)));
        }

        public static IEnumerable<Exercise> SearchByTerm(this IEnumerable<Exercise> exercises, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return exercises;

            searchTerm = searchTerm.Trim().ToLower();

            return exercises.Where(e =>
                e.Name.ToLower().Contains(searchTerm) ||
                (e.Description != null && e.Description.ToLower().Contains(searchTerm)) ||
                (e.Instructions != null && e.Instructions.ToLower().Contains(searchTerm)) ||
                (e.Category != null && e.Category.ToLower().Contains(searchTerm)) ||
                (e.Equipment != null && e.Equipment.ToLower().Contains(searchTerm)) ||
                (e.TargetMuscles != null && e.TargetMuscles.Any(tm =>
                    tm.MuscleGroup.ToLower().Contains(searchTerm) ||
                    (tm.Muscle != null && tm.Muscle.DisplayName.ToLower().Contains(searchTerm)))));
        }
    }
}