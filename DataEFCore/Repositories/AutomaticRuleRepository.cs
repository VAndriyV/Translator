using Domain.Entities;
using Domain.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Linq.Expressions;

namespace DataEFCore.Repositories
{
    public class AutomaticRuleRepository : IAutomaticRuleRepository
    {
        private readonly IDbContext _context;

        public AutomaticRuleRepository(IDbContext context)
        {
            _context = context;
        }

        public async Task<List<AutomaticRule>> GetAll()
        {
            return await _context.AutomaticRules.ToListAsync();
        }

        public async Task<AutomaticRule> Find(Func<AutomaticRule, bool> predicate)
        {
            return await _context.AutomaticRules.FirstOrDefaultAsync(x => predicate(x));
        }
    }
}
