

// FitnessTracker.Exercises.Core/Entities/ExerciseTargetMuscle.cs
using System;

namespace FitnessTracker.Exercises.Core.Entities
{
    public class ExerciseTargetMuscle
    {
        public int ExerciseId { get; set; }
        public string MuscleGroup { get; set; }
        public bool IsPrimary { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public virtual Exercise Exercise { get; set; }
        public virtual MuscleGroup Muscle { get; set; }
    }
}