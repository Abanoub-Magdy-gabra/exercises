

// FitnessTracker.Exercises.API/Models/Responses/SearchResultResponse.cs
using System.Collections.Generic;

namespace FitnessTracker.Exercises.API.Models.Responses
{
    public class SearchResultResponse
    {
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public bool HasMore { get; set; }
        public List<ExerciseResponse> Exercises { get; set; } = new List<ExerciseResponse>();
    }
}