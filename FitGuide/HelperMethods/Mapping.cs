using AutoMapper;
using Core;
using Core.Identity.Entities;
using FitGuide.DTOs;

namespace FitGuide.HelperMethods
{
    public class Mapping:Profile
    {
        public Mapping()
        {
            CreateMap<UserMetrics, UserMetricsDTO>().ReverseMap()
                .ForMember(um=>um.BMI,opt=>opt.Ignore()).ForMember(um => um.CreatedAt, opt => opt.Ignore())
                .ForMember(um=>um.fitnessLevel,opt=>opt.MapFrom(um=>um.fitnessLevel.ToString()));

            CreateMap<UpdateUserMetricsDTO, UserMetrics>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, srcmember) => src != null));
            CreateMap<UserGoalDTO, UserGoal>().ReverseMap();
            CreateMap<InjuryUserDTO, UserInjury>().ReverseMap();
            CreateMap<WorkOutExercises, WorkOutExercisesResponseDTO>()
                           .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                           .ForMember(dest => dest.ExerciseId, opt => opt.MapFrom(src => src.ExerciseId))
                           .ForMember(dest => dest.WorkoOutId, opt => opt.MapFrom(src => src.WorkoOutId))
                           .ForMember(dest => dest.WorkOutName, opt => opt.MapFrom(src => src.workOutPlan.Name))
                           .ForMember(dest => dest.NumberOfReps, opt => opt.MapFrom(src => src.NumberOfReps))
                           .ForMember(dest => dest.NumberOfSets, opt => opt.MapFrom(src => src.NumberOfSets))
                           .ForMember(dest => dest.Exercise, opt => opt.MapFrom(src => src.exercise))
                           .ForMember(dest => dest.WorkOutPlan, opt => opt.MapFrom(src => src.workOutPlan));
                                

            // Map Exercise to ExerciseDto
            CreateMap<Exercise, ExerciseDto>()
                .ForMember(dest => dest.Difficulty, opt => opt.MapFrom(src => src.Difficulty.ToString()));

            // Map WorkOutPlan to WorkOutPlanDto
            CreateMap<WorkOutPlan, WorkOutPlanDto>()
                .ForMember(dest => dest.DifficultyLevel, opt => opt.MapFrom(src => src.DifficultyLevel.ToString()));


        }
    }
}
