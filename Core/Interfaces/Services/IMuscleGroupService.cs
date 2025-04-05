
// FitnessTracker.Exercises.Core/Interfaces/Services/IMuscleGroupService.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using FitnessTracker.Exercises.Core.Entities;

namespace Workout.Core.Interfaces.Services
{
    public interface IMuscleGroupService
    {
        Task<IEnumerable<MuscleGroup>> GetAllMuscleGroupsAsync();
        Task<MuscleGroup> GetMuscleGroupByNameAsync(string name);
        Task<IEnumerable<MuscleGroup>> GetMuscleGroupsByBodyPartAsync(string bodyPart);
        Task<IEnumerable<string>> GetAllBodyPartsAsync();
        Task<IDictionary<string, IEnumerable<MuscleGroup>>> GetMuscleGroupsByBodyPartOrganizedAsync();
    }
}