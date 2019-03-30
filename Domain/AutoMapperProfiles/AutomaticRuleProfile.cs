using AutoMapper;
using Domain.Entities;
using Domain.ViewModels;

namespace Domain.AutoMapperProfiles
{
    public class AutomaticRuleProfile : Profile
    {
        public AutomaticRuleProfile()
        {
            CreateMap<AutomaticRule, AutomaticRuleViewModel>();
        }
    }
}
