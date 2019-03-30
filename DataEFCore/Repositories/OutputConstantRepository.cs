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
    public class OutputConstantRepository : IOutputConstantRepository
    {
        private readonly IDbContext _context;

        private const string TABLE_NAME = "OutputConstants";

        public OutputConstantRepository(IDbContext context)
        {
            _context = context;
        }

        public async Task<List<OutputConstant>> GetAll()
        {
            return await _context.OutputConstants.ToListAsync();
        }

        public async Task<OutputConstant> Find(Func<OutputConstant, bool> predicate)
        {
            return await _context.OutputConstants.FirstOrDefaultAsync(x => predicate(x));
        }

        public async Task Add(OutputConstant constant)
        {
            await _context.OutputConstants.AddAsync(constant);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ConstantExist(string constant)
        {
            return await _context.OutputConstants.AnyAsync(l => l.Name == constant);
        }

        public async Task<int> GetCode(string constant)
        {
            var entity = await _context.OutputConstants.FirstOrDefaultAsync(l => l.Name == constant);
            return entity.Id;
        }

        public async Task DeleteAllRows()
        {
            await _context.TruncateTable(TABLE_NAME);
            await _context.SaveChangesAsync();
        }
    }
}
