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
    public class OutputLexemeRepository : IOutputLexemeRepository
    {
        private readonly IDbContext _context;

        private const string TABLE_NAME = "OutputLexemes";

        public OutputLexemeRepository(IDbContext context)
        {
            _context = context;
        }

        public async Task<List<OutputLexeme>> GetAll()
        {
            return await _context.OutputLexemes.ToListAsync();
        }

        public async Task<OutputLexeme> Find(Func<OutputLexeme, bool> predicate)
        {
            return await _context.OutputLexemes.FirstOrDefaultAsync(x => predicate(x));
        }

        public async Task Add(OutputLexeme lexeme)
        {
            await _context.OutputLexemes.AddAsync(lexeme);
            await _context.SaveChangesAsync();
        }

        public async Task<OutputLexeme> GetLast()
        {
            return await _context.OutputLexemes.LastOrDefaultAsync();
        }

        public async Task DeleteAllRows()
        {
            await _context.TruncateTable(TABLE_NAME);
            await _context.SaveChangesAsync();
        }
    }
}
