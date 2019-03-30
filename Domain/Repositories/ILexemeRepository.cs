using Domain.Entities;
using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface ILexemeRepository
    {
        Task<List<Lexeme>> GetAll();
        Task<Lexeme> Find(Func<Lexeme, bool> predicate);
        Task<bool> LexemeExist(string lexeme);
        Task<int> GetCode(string lexeme);
    }
}
