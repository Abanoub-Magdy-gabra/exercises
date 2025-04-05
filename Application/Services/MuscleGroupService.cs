
// FitnessTracker.Exercises.Application/Services/MuscleGroupService.cs
using FitnessTracker.Exercises.Core.Entities;
using Workout.Core.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workout.Core.Interfaces.Repositories;

namespace Workout.Application.Services
{
    public class MuscleGroupService : IMuscleGroupService
    {
        private readonly IMuscleGroupRepository _muscleGroupRepository;
        private readonly ILogger<MuscleGroupService> _logger;

        public MuscleGroupService(
            IMuscleGroupRepository muscleGroupRepository,
            ILogger<MuscleGroupService> logger)
        {
            _muscleGroupRepository = muscleGroupRepository ?? throw new ArgumentNullException(nameof(muscleGroupRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<MuscleGroup>> GetAllMuscleGroupsAsync()
        {
            try
            {
                return await _muscleGroupRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all muscle groups");
                throw;
            }
        }

        public async Task<MuscleGroup> GetMuscleGroupByNameAsync(string name)
        {
            try
            {
                return await _muscleGroupRepository.GetByNameAsync(name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving muscle group {MuscleGroup}", name);
                throw;
            }
        }

        public async Task<IEnumerable<MuscleGroup>> GetMuscleGroupsByBodyPartAsync(string bodyPart)
        {
            try
            {
                return await _muscleGroupRepository.GetByBodyPartAsync(bodyPart);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving muscle groups for body part {BodyPart}", bodyPart);
                throw;
            }
        }

        public async Task<IEnumerable<string>> GetAllBodyPartsAsync()
        {
            try
            {
                var muscleGroups = await _muscleGroupRepository.GetAllAsync();
                return muscleGroups.Select(m => m.BodyPart).Distinct().ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all body parts");
                throw;
            }
        }

        public async Task<IDictionary<string, IEnumerable<MuscleGroup>>> GetMuscleGroupsByBodyPartOrganizedAsync()
        {
            try
            {
                var muscleGroups = await _muscleGroupRepository.GetAllAsync();
                return muscleGroups
                    .GroupBy(m => m.BodyPart)
                    .ToDictionary(g => g.Key, g => g.AsEnumerable());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving organized muscle groups by body part");
                throw;
            }
        }
    }
}
