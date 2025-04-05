

// FitnessTracker.Exercises.API/Models/Responses/ExerciseCategoryResponse.cs
using System.Collections.Generic;

namespace FitnessTracker.Exercises.API.Models.Responses
{
    public class ExerciseCategoryResponse
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IconUrl { get; set; }
        public int SortOrder { get; set; }
        public int? ParentCategoryId { get; set; }
        public List<ExerciseCategoryResponse> SubCategories { get; set; } = new List<ExerciseCategoryResponse>();
    }
}
