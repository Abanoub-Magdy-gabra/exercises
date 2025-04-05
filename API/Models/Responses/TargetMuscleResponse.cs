
// FitnessTracker.Exercises.API/Models/Responses/TargetMuscleResponse.cs
namespace FitnessTracker.Exercises.API.Models.Responses
{
    public class TargetMuscleResponse
    {
        public string MuscleGroup { get; set; }
        public string DisplayName { get; set; }
        public string BodyPart { get; set; }
        public bool IsPrimary { get; set; }
        public string ImageUrl { get; set; }
    }
}