
// FitnessTracker.Exercises.Core/Interfaces/Services/IExerciseService.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using FitnessTracker.Exercises.Core.Entities;
using FitnessTracker.Exercises.Core.Enums;

namespace Workout.Core.Interfaces.Services
{
    public interface IExerciseService
    {
        Task<IEnumerable<Exercise>> GetAllExercisesAsync();
        Task<Exercise> GetExerciseByIdAsync(int id);
        Task<Exercise> GetExerciseDetailByIdAsync(int id);
        Task<IEnumerable<Exercise>> GetExercisesByCategoryAsync(string category);
        Task<IEnumerable<Exercise>> GetExercisesByMuscleGroupAsync(string muscleGroup);
        Task<IEnumerable<Exercise>> GetExercisesByDifficultyAsync(ExerciseDifficultyEnum difficulty);
        Task<IEnumerable<Exercise>> GetExercisesByEquipmentAsync(string equipment);
        Task<Exercise> CreateExerciseAsync(Exercise exercise);
        Task<Exercise> UpdateExerciseAsync(int id, Exercise exercise);
        Task<bool> DeleteExerciseAsync(int id);
        Task<bool> VerifyExerciseAsync(int id);
    }
}
