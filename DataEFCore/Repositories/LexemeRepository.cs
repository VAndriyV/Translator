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
    public class LexemeRepository : ILexemeRepository
    {
        private readonly IDbContext _context;

        public LexemeRepository(IDbContext context)
        {
            _context = context;
        }

        public async Task<List<Lexeme>> GetAll()
        {
            return await _context.Lexemes.ToListAsync();
        }

        public async Task<Lexeme> Find(Func<Lexeme, bool> predicate)
        {            
            return await _context.Lexemes.FirstOrDefaultAsync(x=>predicate(x));
        }

        public async Task<bool> LexemeExist(string lexeme)
        {
            return await _context.Lexemes.AnyAsync(l => l.Name == lexeme);
        }

        public async Task<int> GetCode(string lexeme)
        {
            var entity =  await _context.Lexemes.FirstOrDefaultAsync(l => l.Name == lexeme);
            return entity.Id;
        }
    }
}
