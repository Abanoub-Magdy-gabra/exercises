
// FitnessTracker.Exercises.API/Models/Requests/UpdateExerciseMediaRequest.cs
using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Exercises.API.Models.Requests
{
    public class UpdateExerciseMediaRequest
    {
        [StringLength(255)]
        public string Description { get; set; }
    }
}