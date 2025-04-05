
// FitnessTracker.Exercises.API/Models/Mappers/ExerciseMapperProfile.cs
using AutoMapper;
using FitnessTracker.Exercises.API.Models.Requests;
using FitnessTracker.Exercises.API.Models.Responses;
using FitnessTracker.Exercises.Core.Entities;
using FitnessTracker.Exercises.Core.Enums;
using System.Linq;

namespace Workout.API.Models.Responses
{
    public class ExerciseMapperProfile : Profile
    {
        public ExerciseMapperProfile(Exercise src, Exercise f)
        {
            // Map from Entity to Response
            if (src.Media == null || !src.Media.Any())
            {
                // Map from Entity to Response
                CreateMap<Exercise, ExerciseResponse>()
                .ForMember(dest => dest.Difficulty, opt => opt.MapFrom(src => src.Difficulty.ToString()))
                .ForMember(dest => dest.PrimaryImageUrl, opt => opt.MapFrom(src =>
                    (string?)null))
                .ForMember(dest => dest.PrimaryMuscleGroups, opt => opt.MapFrom(src =>
                    src.TargetMuscles != null ?
                    src.TargetMuscles.Where(tm => tm.IsPrimary).Select(tm => tm.MuscleGroup).ToArray() :
                    new string[0]));
            }
            else
            {
                // Map from Entity to Response
                CreateMap<Exercise, ExerciseResponse>()
               .ForMember(dest => dest.PrimaryImageUrl, static opt => opt.MapFrom(src =>
    src.Media != null ?
    (src.Media.FirstOrDefault(m => m.MediaType == MediaTypeEnum.Image) != null ?
        src.Media.FirstOrDefault(m => m.MediaType == MediaTypeEnum.Image).Url :
        null) :
    null));
            }

            CreateMap<Exercise, ExerciseDetailResponse>()
                .ForMember(dest => dest.Difficulty, opt => opt.MapFrom(src => src.Difficulty.ToString()));

            CreateMap<ExerciseMedia, ExerciseMediaResponse>()
                .ForMember(dest => dest.MediaType, opt => opt.MapFrom(src => src.MediaType.ToString()));

            CreateMap<ExerciseTargetMuscle, TargetMuscleResponse>()
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Muscle != null ? src.Muscle.DisplayName : src.MuscleGroup))
                .ForMember(dest => dest.BodyPart, opt => opt.MapFrom(src => src.Muscle != null ? src.Muscle.BodyPart : null))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Muscle != null ? src.Muscle.ImageUrl : null));

            CreateMap<ExerciseCategory, ExerciseCategoryResponse>();

            // Map from Request to Entity
            CreateMap<CreateExerciseRequest, Exercise>()
                .ForMember(dest => dest.TargetMuscles, opt => opt.Ignore())
                .ForMember(dest => dest.Media, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<UpdateExerciseRequest, Exercise>()
                .ForMember(dest => dest.ExerciseId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.TargetMuscles, opt => opt.Ignore())
                .ForMember(dest => dest.Media, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<AddTargetMuscleRequest, ExerciseTargetMuscle>()
                .ForMember(dest => dest.Exercise, opt => opt.Ignore())
                .ForMember(dest => dest.Muscle, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
        }
    }
}