using AutoMapper;
using Domain.Entities;
using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.AutoMapperProfiles
{
    public class OutputIdnViewModelProfile : Profile
    {
        public OutputIdnViewModelProfile()
        {
            CreateMap<OutputIdnViewModel, OutputIdn>();
        }
    }
}
