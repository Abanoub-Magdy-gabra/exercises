
// FitnessTracker.Exercises.API/Models/Requests/AddTargetMuscleRequest.cs
using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Exercises.API.Models.Requests
{
    public class AddTargetMuscleRequest
    {
        [Required]
        [StringLength(50)]
        public string MuscleGroup { get; set; }

        public bool IsPrimary { get; set; } = false;
    }
}