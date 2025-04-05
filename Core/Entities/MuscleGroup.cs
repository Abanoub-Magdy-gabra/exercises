

// FitnessTracker.Exercises.Core/Entities/MuscleGroup.cs
using System.Collections.Generic;

namespace FitnessTracker.Exercises.Core.Entities
{
    public class MuscleGroup
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string BodyPart { get; set; }
        public string ImageUrl { get; set; }

        // Navigation property
        public virtual ICollection<ExerciseTargetMuscle> Exercises { get; set; }
    }
}
