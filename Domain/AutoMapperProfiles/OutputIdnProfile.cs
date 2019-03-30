using AutoMapper;
using Domain.Entities;
using Domain.ViewModels;

namespace Domain.AutoMapperProfiles
{
    public class OutputIdnProfile : Profile
    {
        public OutputIdnProfile()
        {
            CreateMap<OutputIdn, OutputIdnViewModel>();
        }
    }
}
