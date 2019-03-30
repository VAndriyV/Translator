using AutoMapper;
using Domain.Entities;
using Domain.ViewModels;

namespace Domain.AutoMapperProfiles
{
    public class OutputLexemeProfile : Profile
    {
        public OutputLexemeProfile()
        {
            CreateMap<OutputLexeme, OutputLexemeViewModel>();
        }
    }
}
