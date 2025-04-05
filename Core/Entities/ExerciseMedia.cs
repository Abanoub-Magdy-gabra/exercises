// FitnessTracker.Exercises.Core/Entities/ExerciseMedia.cs
using System;
using FitnessTracker.Exercises.Core.Enums;

namespace FitnessTracker.Exercises.Core.Entities
{
    public class ExerciseMedia
    {
        public int MediaId { get; set; }
        public int ExerciseId { get; set; }
        public MediaTypeEnum MediaType { get; set; }
        public string Url { get; set; }
        public string ThumbnailUrl { get; set; }
        public string FileName { get; set; }
        public long? FileSize { get; set; }
        public string ContentType { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation property
        public virtual Exercise Exercise { get; set; }
    }
}