using AutoMapper;
using Domain.Entities;
using Domain.ViewModels;

namespace Domain.AutoMapperProfiles
{
    public class OutputConstantProfile : Profile
    {
        public OutputConstantProfile()
        {
            CreateMap<OutputConstant, OutputConstantViewModel>();
        }
    }
}
