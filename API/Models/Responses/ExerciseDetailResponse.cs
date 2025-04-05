
// FitnessTracker.Exercises.API/Models/Responses/ExerciseDetailResponse.cs
using System;
using System.Collections.Generic;
using FitnessTracker.Exercises.Core.Enums;

namespace FitnessTracker.Exercises.API.Models.Responses
{
    public class ExerciseDetailResponse
    {
        public int ExerciseId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Equipment { get; set; }
        public string Difficulty { get; set; }
        public string Instructions { get; set; }
        public bool IsVerified { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<TargetMuscleResponse> TargetMuscles { get; set; } = new List<TargetMuscleResponse>();
        public List<ExerciseMediaResponse> Media { get; set; } = new List<ExerciseMediaResponse>();
    }
}
