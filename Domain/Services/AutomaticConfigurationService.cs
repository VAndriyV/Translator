using Domain.Entities;
using Domain.Repositories;
using Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class AutomaticConfigurationService : IAutomaticConfigurationService
    {
        private readonly IAutomaticRuleRepository _ruleRepository;

        public AutomaticConfigurationService(IAutomaticRuleRepository ruleRepository)
        {
            _ruleRepository = ruleRepository;
        }

        public async Task<List<AutomaticRule>> GetAutomaticRules()
        {
            return await _ruleRepository.GetAll();
        }
    }
}
