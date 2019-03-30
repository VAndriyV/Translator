using AutoMapper;
using Domain.Entities;
using Domain.ViewModels;

namespace Domain.AutoMapperProfiles
{
    public class OutputLexemeViewModelProfile : Profile
    {
        public OutputLexemeViewModelProfile()
        {
            CreateMap<OutputLexemeViewModel, OutputLexeme>();
        }
    }
}
