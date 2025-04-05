

// FitnessTracker.Exercises.API/Controllers/SearchController.cs
using AutoMapper;
using FitnessTracker.Exercises.API.Models.Requests;
using FitnessTracker.Exercises.API.Models.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workout.Core.Interfaces.Services;

namespace WebApplication1.API.Controllers
{
    [ApiController]
    [Route("api/exercises/search")]
    public class SearchController : ControllerBase
    {
        private readonly IExerciseSearchService _searchService;
        private readonly IMapper _mapper;
        private readonly ILogger<SearchController> _logger;

        public SearchController(
            IExerciseSearchService searchService,
            IMapper mapper,
            ILogger<SearchController> logger)
        {
            _searchService = searchService ?? throw new ArgumentNullException(nameof(searchService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(typeof(SearchResultResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchExercises([FromQuery] SearchExercisesRequest request)
        {
            var exercises = string.IsNullOrWhiteSpace(request.Query)
                ? await _searchService.SearchAdvancedAsync(
                    null,
                    request.Category,
                    request.MuscleGroup,
                    request.Difficulty,
                    request.Equipment)
                : await _searchService.SearchAdvancedAsync(
                    request.Query,
                    request.Category,
                    request.MuscleGroup,
                    request.Difficulty,
                    request.Equipment);

            // Apply pagination
            var pageSize = request.Limit ?? 10;
            var page = request.Page ?? 1;
            var totalCount = exercises.Count();
            var pagedExercises = exercises
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var exerciseResponses = _mapper.Map<List<ExerciseResponse>>(pagedExercises);

            var response = new SearchResultResponse
            {
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                HasMore = page * pageSize < totalCount,
                Exercises = exerciseResponses
            };

            return Ok(response);
        }

        [HttpGet("popular")]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPopularSearchTerms([FromQuery] int limit = 10)
        {
            var popularTerms = await _searchService.GetPopularSearchTermsAsync(limit);
            return Ok(popularTerms);
        }

        [HttpGet("recommended")]
        [ProducesResponseType(typeof(IEnumerable<ExerciseResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRecommendedExercises([FromQuery] int limit = 10)
        {
            var userId = GetCurrentUserId();
            var exercises = await _searchService.GetRecommendedExercisesAsync(userId, limit);
            var response = _mapper.Map<IEnumerable<ExerciseResponse>>(exercises);
            return Ok(response);
        }

        private int GetCurrentUserId()
        {
            // Implementation will depend on your authentication mechanism
            // This is a placeholder implementation
            return int.Parse(User.FindFirst("sub")?.Value ?? "0");
        }
    }
}
 