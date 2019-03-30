using Domain.Entities;
using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface ILexemeClassRepository
    {
        Task<List<LexemeClass>> GetAll();
        Task<LexemeClass> Find(Func<LexemeClass, bool> predicate);
    }
}
