using AutoMapper;
using Domain.Entities;
using Domain.ViewModels;

namespace Domain.AutoMapperProfiles
{
    public class LexemeProfile : Profile
    {
        public LexemeProfile()
        {
            CreateMap<Lexeme, LexemeViewModel>();
        }
    }
}
