using Domain.Entities;
using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IOutputLexemeRepository
    {
        Task<List<OutputLexeme>> GetAll();
        Task<OutputLexeme> Find(Func<OutputLexeme, bool> predicate);
        Task Add(OutputLexeme lexeme);
        Task<OutputLexeme> GetLast();
        Task DeleteAllRows();
    }
}
