using AutoMapper;
using Domain.Entities;
using Domain.ViewModels;

namespace Domain.AutoMapperProfiles
{
    class LexemeClassProfile : Profile
    {
        public LexemeClassProfile()
        {
            CreateMap<LexemeClass, LexemeClassViewModel>();
        }
    }
}
