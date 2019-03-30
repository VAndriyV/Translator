using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Util.Interfaces
{
    public interface ICharacterClassDeterminant
    {
        Task<bool> IsLetter(char ch);
      
        Task<bool> IsDigit(char ch);
       
        Task<bool> IsExclamation(char ch);
       
        Task<bool> IsLess(char ch);
        
        Task<bool> IsMore(char ch);
        
        Task<bool> IsEqual(char ch);
        
        Task<bool> IsSeparator(char ch);
        
        Task<bool> IsSign(char ch);        
    }
}
