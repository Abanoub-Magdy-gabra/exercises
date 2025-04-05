
// FitnessTracker.Exercises.Core/Entities/ExerciseCategory.cs
using System.Collections.Generic;

namespace FitnessTracker.Exercises.Core.Entities
{
    public class ExerciseCategory
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IconUrl { get; set; }
        public int SortOrder { get; set; }
        public int? ParentCategoryId { get; set; }

        // Navigation properties
        public virtual ExerciseCategory ParentCategory { get; set; }
        public virtual ICollection<ExerciseCategory> SubCategories { get; set; }
    }
}
