

// FitnessTracker.Exercises.Core/Interfaces/Repositories/IMuscleGroupRepository.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using FitnessTracker.Exercises.Core.Entities;

namespace Workout.Core.Interfaces.Repositories
{
    public interface IMuscleGroupRepository
    {
        Task<IEnumerable<MuscleGroup>> GetAllAsync();
        Task<MuscleGroup> GetByNameAsync(string name);
        Task<IEnumerable<MuscleGroup>> GetByBodyPartAsync(string bodyPart);
        Task<bool> ExistsAsync(string name);
    }
}
