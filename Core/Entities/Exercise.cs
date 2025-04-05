// FitnessTracker.Exercises.Core/Entities/Exercise.cs
using System;
using System.Collections.Generic;
using FitnessTracker.Exercises.Core.Enums;

namespace FitnessTracker.Exercises.Core.Entities
{
    public class Exercise
    {
        public int ExerciseId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Equipment { get; set; }
        public ExerciseDifficultyEnum Difficulty { get; set; }
        public string Instructions { get; set; }
        public bool IsVerified { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public virtual ICollection<ExerciseTargetMuscle> TargetMuscles { get; set; }
        public virtual ICollection<ExerciseMedia> Media { get; set; }
    }
}


