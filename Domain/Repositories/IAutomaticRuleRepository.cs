using Domain.Entities;
using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IAutomaticRuleRepository
    {
        Task<List<AutomaticRule>> GetAll();
        Task<AutomaticRule> Find(Func<AutomaticRule, bool> predicate);
    }
}
