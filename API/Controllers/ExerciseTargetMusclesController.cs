

// FitnessTracker.Exercises.API/Controllers/ExerciseTargetMusclesController.cs
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
    [Route("api/exercises/{exerciseId}/muscles")]
    public class ExerciseTargetMusclesController : ControllerBase
    {
        private readonly IExerciseService _exerciseService;
        private readonly IExerciseTargetMuscleRepository _targetMuscleRepository;
        private readonly IMuscleGroupRepository _muscleGroupRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ExerciseTargetMusclesController> _logger;

        public ExerciseTargetMusclesController(
            IExerciseService exerciseService,
            IExerciseTargetMuscleRepository targetMuscleRepository,
            IMuscleGroupRepository muscleGroupRepository,
            IMapper mapper,
            ILogger<ExerciseTargetMusclesController> logger)
        {
            _exerciseService = exerciseService ?? throw new ArgumentNullException(nameof(exerciseService));
            _targetMuscleRepository = targetMuscleRepository ?? throw new ArgumentNullException(nameof(targetMuscleRepository));
            _muscleGroupRepository = muscleGroupRepository ?? throw new ArgumentNullException(nameof(muscleGroupRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TargetMuscleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTargetMuscles(int exerciseId)
        {
            var exercise = await _exerciseService.GetExerciseByIdAsync(exerciseId);
            if (exercise == null)
            {
                return NotFound();
            }

            var targetMuscles = await _targetMuscleRepository.GetByExerciseIdAsync(exerciseId);
            var response = _mapper.Map<IEnumerable<TargetMuscleResponse>>(targetMuscles);
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Coach")]
        [ProducesResponseType(typeof(TargetMuscleResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> AddTargetMuscle(int exerciseId, [FromBody] AddTargetMuscleRequest request)
        {
            var exercise = await _exerciseService.GetExerciseByIdAsync(exerciseId);
            if (exercise == null)
            {
                return NotFound();
            }

            var muscleGroup = await _muscleGroupRepository.GetByNameAsync(request.MuscleGroup);
            if (muscleGroup == null)
            {
                return BadRequest($"Muscle group '{request.MuscleGroup}' not found");
            }

            var exists = await _targetMuscleRepository.ExistsAsync(exerciseId, request.MuscleGroup);
            if (exists)
            {
                return Conflict($"Muscle group '{request.MuscleGroup}' is already associated with this exercise");
            }

            var targetMuscle = new ExerciseTargetMuscle
            {
                ExerciseId = exerciseId,
                MuscleGroup = request.MuscleGroup,
                IsPrimary = request.IsPrimary,
                CreatedAt = DateTime.UtcNow
            };

            var createdTargetMuscle = await _targetMuscleRepository.AddAsync(targetMuscle);

            // Get the complete object with navigation properties
            var allTargetMuscles = await _targetMuscleRepository.GetByExerciseIdAsync(exerciseId);
            var addedTargetMuscle = allTargetMuscles.FirstOrDefault(tm => tm.MuscleGroup == request.MuscleGroup);

            var response = _mapper.Map<TargetMuscleResponse>(addedTargetMuscle);
            return CreatedAtAction(nameof(GetTargetMuscles), new { exerciseId }, response);
        }

        [HttpPut("{muscleGroup}/primary/{isPrimary}")]
        [Authorize(Roles = "Admin,Coach")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePrimaryStatus(int exerciseId, string muscleGroup, bool isPrimary)
        {
            var exists = await _targetMuscleRepository.ExistsAsync(exerciseId, muscleGroup);
            if (!exists)
            {
                return NotFound($"Muscle group '{muscleGroup}' is not associated with this exercise");
            }

            await _targetMuscleRepository.UpdatePrimaryStatusAsync(exerciseId, muscleGroup, isPrimary);
            return Ok();
        }

        [HttpDelete("{muscleGroup}")]
        [Authorize(Roles = "Admin,Coach")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveTargetMuscle(int exerciseId, string muscleGroup)
        {
            var exists = await _targetMuscleRepository.ExistsAsync(exerciseId, muscleGroup);
            if (!exists)
            {
                return NotFound($"Muscle group '{muscleGroup}' is not associated with this exercise");
            }

            await _targetMuscleRepository.RemoveAsync(exerciseId, muscleGroup);
            return NoContent();
        }
    }
}

