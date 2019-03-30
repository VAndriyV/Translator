using Domain.Entities;
using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IOutputConstantRepository
    {
        Task<List<OutputConstant>> GetAll();
        Task<OutputConstant> Find(Func<OutputConstant, bool> predicate);
        Task Add(OutputConstant constant);
        Task<bool> ConstantExist(string constant);
        Task<int> GetCode(string constant);
        Task DeleteAllRows();
    }
}
