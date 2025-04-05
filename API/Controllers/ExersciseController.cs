// FitnessTracker.Exercises.API/Controllers/ExercisesController.cs
using AutoMapper;
using FitnessTracker.Exercises.API.Models.Requests;
using FitnessTracker.Exercises.API.Models.Responses;
using FitnessTracker.Exercises.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workout.Core.Interfaces.Repositories;
using Workout.Core.Interfaces.Services;

namespace WebApplication1.API.Controllers
{
    [ApiController]
    [Route("api/exercises")]
    public class ExercisesController : ControllerBase
    {
        private readonly IExerciseService _exerciseService;
        private readonly IExerciseTargetMuscleRepository _targetMuscleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ExercisesController> _logger;

        public ExercisesController(
            IExerciseService exerciseService,
            IExerciseTargetMuscleRepository targetMuscleRepository,
            IMapper mapper,
            ILogger<ExercisesController> logger)
        {
            _exerciseService = exerciseService ?? throw new ArgumentNullException(nameof(exerciseService));
            _targetMuscleRepository = targetMuscleRepository ?? throw new ArgumentNullException(nameof(targetMuscleRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ExerciseResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllExercises()
        {
            var exercises = await _exerciseService.GetAllExercisesAsync();
            var response = _mapper.Map<IEnumerable<ExerciseResponse>>(exercises);
            return Ok(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ExerciseDetailResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetExerciseById(int id)
        {
            var exercise = await _exerciseService.GetExerciseDetailByIdAsync(id);
            if (exercise == null)
            {
                return NotFound();
            }

            var response = _mapper.Map<ExerciseDetailResponse>(exercise);
            return Ok(response);
        }

        [HttpGet("category/{category}")]
        [ProducesResponseType(typeof(IEnumerable<ExerciseResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetExercisesByCategory(string category)
        {
            var exercises = await _exerciseService.GetExercisesByCategoryAsync(category);
            var response = _mapper.Map<IEnumerable<ExerciseResponse>>(exercises);
            return Ok(response);
        }

        [HttpGet("muscle/{muscleGroup}")]
        [ProducesResponseType(typeof(IEnumerable<ExerciseResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetExercisesByMuscleGroup(string muscleGroup)
        {
            var exercises = await _exerciseService.GetExercisesByMuscleGroupAsync(muscleGroup);
            var response = _mapper.Map<IEnumerable<ExerciseResponse>>(exercises);
            return Ok(response);
        }

        [HttpGet("difficulty/{difficulty}")]
        [ProducesResponseType(typeof(IEnumerable<ExerciseResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetExercisesByDifficulty(string difficulty)
        {
            if (!Enum.TryParse<FitnessTracker.Exercises.Core.Enums.ExerciseDifficultyEnum>(difficulty, true, out var difficultyEnum))
            {
                return BadRequest("Invalid difficulty level");
            }

            var exercises = await _exerciseService.GetExercisesByDifficultyAsync(difficultyEnum);
            var response = _mapper.Map<IEnumerable<ExerciseResponse>>(exercises);
            return Ok(response);
        }

        [HttpGet("equipment/{equipment}")]
        [ProducesResponseType(typeof(IEnumerable<ExerciseResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetExercisesByEquipment(string equipment)
        {
            var exercises = await _exerciseService.GetExercisesByEquipmentAsync(equipment);
            var response = _mapper.Map<IEnumerable<ExerciseResponse>>(exercises);
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Coach")]
        [ProducesResponseType(typeof(ExerciseDetailResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateExercise([FromBody] CreateExerciseRequest request)
        {
            var exercise = _mapper.Map<Exercise>(request);
            exercise.CreatedBy = GetCurrentUserId();

            var createdExercise = await _exerciseService.CreateExerciseAsync(exercise);

            // Add target muscles
            if (request.TargetMuscles != null && request.TargetMuscles.Any())
            {
                foreach (var targetMuscle in request.TargetMuscles)
                {
                    await _targetMuscleRepository.AddAsync(new ExerciseTargetMuscle
                    {
                        ExerciseId = createdExercise.ExerciseId,
                        MuscleGroup = targetMuscle.MuscleGroup,
                        IsPrimary = targetMuscle.IsPrimary,
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }

            // Get the complete exercise with all relationships
            var completeExercise = await _exerciseService.GetExerciseDetailByIdAsync(createdExercise.ExerciseId);
            var response = _mapper.Map<ExerciseDetailResponse>(completeExercise);

            return CreatedAtAction(nameof(GetExerciseById), new { id = createdExercise.ExerciseId }, response);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Coach")]
        [ProducesResponseType(typeof(ExerciseDetailResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateExercise(int id, [FromBody] UpdateExerciseRequest request)
        {
            var existingExercise = await _exerciseService.GetExerciseByIdAsync(id);
            if (existingExercise == null)
            {
                return NotFound();
            }

            var exercise = _mapper.Map(request, existingExercise);
            var updatedExercise = await _exerciseService.UpdateExerciseAsync(id, exercise);

            var completeExercise = await _exerciseService.GetExerciseDetailByIdAsync(updatedExercise.ExerciseId);
            var response = _mapper.Map<ExerciseDetailResponse>(completeExercise);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteExercise(int id)
        {
            var exercise = await _exerciseService.GetExerciseByIdAsync(id);
            if (exercise == null)
            {
                return NotFound();
            }

            await _exerciseService.DeleteExerciseAsync(id);
            return NoContent();
        }

        [HttpPost("{id}/verify")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> VerifyExercise(int id)
        {
            var result = await _exerciseService.VerifyExerciseAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }

        private int GetCurrentUserId()
        {
            // Implementation will depend on your authentication mechanism
            // This is a placeholder implementation
            return int.Parse(User.FindFirst("sub")?.Value ?? "0");
        }
    }
}