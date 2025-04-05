
// FitnessTracker.Exercises.API/Models/Responses/ExerciseMediaResponse.cs
using System;
using FitnessTracker.Exercises.Core.Enums;

namespace FitnessTracker.Exercises.API.Models.Responses
{
    public class ExerciseMediaResponse
    {
        public int MediaId { get; set; }
        public int ExerciseId { get; set; }
        public string MediaType { get; set; }
        public string Url { get; set; }
        public string ThumbnailUrl { get; set; }
        public string FileName { get; set; }
        public long? FileSize { get; set; }
        public string ContentType { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
