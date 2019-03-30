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
    public class LexemeClassRepository : ILexemeClassRepository
    {
        private readonly IDbContext _context;

        public LexemeClassRepository(IDbContext context)
        {
            _context = context;
        }

        public async Task<List<LexemeClass>> GetAll()
        {
            return await _context.LexemeClasses.ToListAsync();
        }

        public async Task<LexemeClass> Find(Func<LexemeClass, bool> predicate)
        {
            return await _context.LexemeClasses.FirstOrDefaultAsync(x => predicate(x));
        }
    }
}
