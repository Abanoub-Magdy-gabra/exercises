// FitnessTracker.Exercises.API/Models/Requests/SearchExercisesRequest.cs
using FitnessTracker.Exercises.Core.Enums;

namespace FitnessTracker.Exercises.API.Models.Requests
{
    public class SearchExercisesRequest
    {
        public string Query { get; set; }
        public string Category { get; set; }
        public string MuscleGroup { get; set; }
        public ExerciseDifficultyEnum? Difficulty { get; set; }
        public string Equipment { get; set; }
        public int? Limit { get; set; }
        public int? Page { get; set; }
    }
}
