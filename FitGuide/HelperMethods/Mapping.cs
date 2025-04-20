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
                .ForMember(um=>um.BMI,opt=>opt.Ignore()).ForMember(um => um.CreatedAt, opt => opt.Ignore());
            CreateMap<UpdateUserMetricsDTO, UserMetrics>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, srcmember) => src != null));
            CreateMap<UserGoalDTO, UserGoal>().ReverseMap()
                .ForMember(dest => dest.targetBMI, opt => opt.MapFrom(src => src.GoalTempelate.targetBMI))
                .ForMember(dest => dest.targetMuscleMass, opt => opt.MapFrom(src => src.GoalTempelate.targetMuscleMass))
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.GoalTempelate.name))
                .ForMember(dest => dest.targetWaterMass, opt => opt.MapFrom(src => src.GoalTempelate.targetWaterMass))
                .ForMember(dest => dest.targetWeight, opt => opt.MapFrom(src => src.GoalTempelate.targetWeight))
                .ForMember(dest => dest.description, opt => opt.MapFrom(src => src.GoalTempelate.description));
            CreateMap<InjuryUserDTO, UserInjury>().ReverseMap();



        }
    }
}
