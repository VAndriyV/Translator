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
    public class OutputIdnRepository : IOutputIdnRepository
    {
        private readonly IDbContext _context;

        private const string TABLE_NAME = "OutputIDNs";

        public OutputIdnRepository(IDbContext context)
        {
            _context = context;
        }

        public async Task<List<OutputIdn>> GetAll()
        {
            return await _context.OutputIdns.ToListAsync();
        }

        public async Task<OutputIdn> Find(Func<OutputIdn, bool> predicate)
        {
            return await _context.OutputIdns.FirstOrDefaultAsync(x => predicate(x));
        }

        public async Task Add(OutputIdn idn)
        {
            await _context.OutputIdns.AddAsync(idn);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IdnExist(string idn)
        {
            return await _context.OutputIdns.AnyAsync(l => l.Name == idn);
        }

        public async Task<int> GetCode(string idn)
        {
            var entity = await _context.OutputIdns.FirstOrDefaultAsync(l => l.Name == idn);
            return entity.Id;
        }

        public async Task DeleteAllRows()
        {
            await _context.TruncateTable(TABLE_NAME);
            await _context.SaveChangesAsync();
        }
    }
}
