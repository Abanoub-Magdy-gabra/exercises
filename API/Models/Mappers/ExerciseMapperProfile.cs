using AutoMapper;
using FitnessTracker.Exercises.API.Models.Requests;
using FitnessTracker.Exercises.API.Models.Responses;
using FitnessTracker.Exercises.Core.Entities;
using FitnessTracker.Exercises.Core.Enums;
using System.Linq;

namespace FitnessTracker.Exercises.API.Models.Mappers
{
    public class ExerciseMapperProfile : Profile
    {
        public ExerciseMapperProfile()
        {
            // Map from Entity to Response
           

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