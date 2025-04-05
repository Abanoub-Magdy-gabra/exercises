// FitnessTracker.Exercises.API/Models/Requests/UpdateExerciseRequest.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FitnessTracker.Exercises.Core.Enums;

namespace FitnessTracker.Exercises.API.Models.Requests
{
    public class UpdateExerciseRequest
    {
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        [StringLength(50)]
        public string Category { get; set; }

        [StringLength(50)]
        public string Equipment { get; set; }

        public ExerciseDifficultyEnum? Difficulty { get; set; }

        [StringLength(4000)]
        public string Instructions { get; set; }

        public bool? IsVerified { get; set; }
    }
}
