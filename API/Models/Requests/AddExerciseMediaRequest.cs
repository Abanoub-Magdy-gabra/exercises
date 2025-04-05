// FitnessTracker.Exercises.API/Models/Requests/AddExerciseMediaRequest.cs
using System.ComponentModel.DataAnnotations;
using FitnessTracker.Exercises.Core.Enums;
using Microsoft.AspNetCore.Http;

namespace FitnessTracker.Exercises.API.Models.Requests
{
    public class AddExerciseMediaRequest
    {
        [Required]
        public IFormFile File { get; set; }

        [Required]
        public MediaTypeEnum MediaType { get; set; }

        [StringLength(255)]
        public string Description { get; set; }
    }
}