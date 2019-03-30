using System;
using System.Collections.Generic;
using System.Text;
using Domain.ViewModels;
using Domain.Entities;
using System.Threading.Tasks;

namespace Domain.Services.Interfaces
{
    public interface ILexemeService
    {
        Task<bool> IsKeyWord(string lexeme);

        Task<bool> IsInOutputIdns(string idn);
        
        Task<bool> IsInOutputConstants(string constant);
        
        Task<bool> IsVariableType(string lexeme);
      
        Task<int> GetIdnCode(string lexeme);
        
        Task AddToOutputIdns(string idn, string type, int row, bool isAlreadyAdded);

        Task<int> GetLexemeCode(string lexeme);
       
        Task AddToOutputLexemes(string lexeme, int row);

        Task<int> GetConstantCode(string lexeme);
        
        Task AddToOutputConstants(string constant,int row, bool isAlreadyAdded);
        
        Task<bool> IsInDeclatationSegment();
        
        Task<LAResult> GetResultTables();
        
        Task<bool> IsNextSignBinary();

        Task ClearOutputTables();
    }
}
