

// FitnessTracker.Exercises.API/Controllers/ExerciseMediaController.cs
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
using System.IO;
using System.Threading.Tasks;
using Workout.Core.Interfaces.Services;

namespace WebApplication1.API.Controllers
{
    [ApiController]
    [Route("api/exercises/{exerciseId}/media")]
    public class ExerciseMediaController : ControllerBase
    {
        private readonly IExerciseService _exerciseService;
        private readonly IExerciseMediaService _mediaService;
        private readonly IMapper _mapper;
        private readonly ILogger<ExerciseMediaController> _logger;

        public ExerciseMediaController(
            IExerciseService exerciseService,
            IExerciseMediaService mediaService,
            IMapper mapper,
            ILogger<ExerciseMediaController> logger)
        {
            _exerciseService = exerciseService ?? throw new ArgumentNullException(nameof(exerciseService));
            _mediaService = mediaService ?? throw new ArgumentNullException(nameof(mediaService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ExerciseMediaResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMediaByExerciseId(int exerciseId)
        {
            var exercise = await _exerciseService.GetExerciseByIdAsync(exerciseId);
            if (exercise == null)
            {
                return NotFound();
            }

            var media = await _mediaService.GetMediaByExerciseIdAsync(exerciseId);
            var response = _mapper.Map<IEnumerable<ExerciseMediaResponse>>(media);
            return Ok(response);
        }

        [HttpGet("{mediaId}")]
        [ProducesResponseType(typeof(ExerciseMediaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMediaById(int exerciseId, int mediaId)
        {
            var media = await _mediaService.GetMediaByIdAsync(mediaId);
            if (media == null || media.ExerciseId != exerciseId)
            {
                return NotFound();
            }

            var response = _mapper.Map<ExerciseMediaResponse>(media);
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Coach")]
        [ProducesResponseType(typeof(ExerciseMediaResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddMedia(int exerciseId, [FromForm] AddExerciseMediaRequest request)
        {
            var exercise = await _exerciseService.GetExerciseByIdAsync(exerciseId);
            if (exercise == null)
            {
                return NotFound();
            }

            using var fileStream = request.File.OpenReadStream();
            var media = await _mediaService.AddMediaAsync(
                exerciseId,
                fileStream,
                request.File.FileName,
                request.File.ContentType,
                request.MediaType);

            var response = _mapper.Map<ExerciseMediaResponse>(media);
            return CreatedAtAction(nameof(GetMediaById), new { exerciseId, mediaId = media.MediaId }, response);
        }

        [HttpDelete("{mediaId}")]
        [Authorize(Roles = "Admin,Coach")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteMedia(int exerciseId, int mediaId)
        {
            var media = await _mediaService.GetMediaByIdAsync(mediaId);
            if (media == null || media.ExerciseId != exerciseId)
            {
                return NotFound();
            }

            await _mediaService.DeleteMediaAsync(mediaId);
            return NoContent();
        }
    }
}