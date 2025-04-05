// FitnessTracker.Exercises.API/Models/Requests/CreateExerciseRequest.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FitnessTracker.Exercises.API.Models.Requests;
using FitnessTracker.Exercises.API.Models.Responses;
using FitnessTracker.Exercises.Core.Enums;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FitnessTracker.Exercises.API.Models.Requests
{
    public class CreateExerciseRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        [Required]
        [StringLength(50)]
        public string Category { get; set; }

        [StringLength(50)]
        public string Equipment { get; set; }

        [Required]
        public ExerciseDifficultyEnum Difficulty { get; set; }

        [StringLength(4000)]
        public string Instructions { get; set; }

        public List<AddTargetMuscleRequest> TargetMuscles { get; set; } = new List<AddTargetMuscleRequest>();
    }
}
