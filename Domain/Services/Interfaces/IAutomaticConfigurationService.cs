﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Interfaces
{
    public interface IAutomaticConfigurationService
    {
        Task<List<AutomaticRule>> GetAutomaticRules();
    }
}
