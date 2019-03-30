using Domain.Entities;
using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IOutputIdnRepository
    {
        Task<List<OutputIdn>> GetAll();
        Task<OutputIdn> Find(Func<OutputIdn, bool> predicate);
        Task Add(OutputIdn idn);
        Task<bool> IdnExist(string idn);
        Task<int> GetCode(string idn);
        Task DeleteAllRows();
    }
}
