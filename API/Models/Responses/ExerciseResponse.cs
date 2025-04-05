
// FitnessTracker.Exercises.API/Models/Responses/ExerciseResponse.cs
using System;
using FitnessTracker.Exercises.Core.Enums;

namespace FitnessTracker.Exercises.API.Models.Responses
{
    public class ExerciseResponse
    {
        public int ExerciseId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Equipment { get; set; }
        public string Difficulty { get; set; }
        public bool IsVerified { get; set; }
        public string PrimaryImageUrl { get; set; }
        public string[] PrimaryMuscleGroups { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
