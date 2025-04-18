using AutoMapper;
using Core;
using FitGuide.DTOs;

namespace FitGuide.HelperMethods
{
    public class Mapping:Profile
    {
        public Mapping()
        {
            CreateMap<UserMetrics, UserMetricsDTO>().ReverseMap()
                .ForMember(um=>um.BMI,opt=>opt.Ignore()).ForMember(um => um.CreatedAt, opt => opt.Ignore());
            CreateMap<UpdateUserMetricsDTO, UserMetrics>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, srcmember) => src != null));
            CreateMap<UserGoalDTO, UserGoal>().ReverseMap()
                .ForMember(dest => dest.BMI, opt => opt.MapFrom(src => src.GoalTempelate.targetBMI))
                .ForMember(dest => dest.MuscleMass, opt => opt.MapFrom(src => src.GoalTempelate.targetMuscleMass))
                .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.GoalTempelate.name))
                .ForMember(dest => dest.WaterMass, opt => opt.MapFrom(src => src.GoalTempelate.targetWaterMass))
                .ForMember(dest => dest.weights, opt => opt.MapFrom(src => src.GoalTempelate.targetWeight))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.GoalTempelate.description));



        }
    }
}
