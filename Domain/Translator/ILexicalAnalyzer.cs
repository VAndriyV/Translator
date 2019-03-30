using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Translator
{
    public interface ILexicalAnalyzer
    {
        Task<LAResult> DoLexicalAnalyze(string sourceCode);
    }
}
