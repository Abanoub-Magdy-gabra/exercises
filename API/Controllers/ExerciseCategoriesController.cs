// FitnessTracker.Exercises.API/Controllers/ExerciseCategoriesController.cs
using AutoMapper;
using FitnessTracker.Exercises.API.Models.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Workout.Core.Interfaces.Repositories;

namespace WebApplication1.API.Controllers
{
    [ApiController]
    [Route("api/exercise-categories")]
    public class ExerciseCategoriesController : ControllerBase
    {
        private readonly IExerciseCategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ExerciseCategoriesController> _logger;

        public ExerciseCategoriesController(
            IExerciseCategoryRepository categoryRepository,
            IMapper mapper,
            ILogger<ExerciseCategoriesController> logger)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ExerciseCategoryResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var response = _mapper.Map<IEnumerable<ExerciseCategoryResponse>>(categories);
            return Ok(response);
        }

        [HttpGet("main")]
        [ProducesResponseType(typeof(IEnumerable<ExerciseCategoryResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMainCategories()
        {
            var categories = await _categoryRepository.GetMainCategoriesAsync();
            var response = _mapper.Map<IEnumerable<ExerciseCategoryResponse>>(categories);
            return Ok(response);
        }

        [HttpGet("{categoryId}/subcategories")]
        [ProducesResponseType(typeof(IEnumerable<ExerciseCategoryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSubcategories(int categoryId)
        {
            var exists = await _categoryRepository.ExistsAsync(categoryId);
            if (!exists)
            {
                return NotFound();
            }

            var subcategories = await _categoryRepository.GetSubcategoriesAsync(categoryId);
            var response = _mapper.Map<IEnumerable<ExerciseCategoryResponse>>(subcategories);
            return Ok(response);
        }
    }
}