using Domain.Entities;
using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Translator
{
    public interface IAutomaticAnalyzer
    {
        Task<AutomaticAnalyzerSteps> DoAnalyze(List<OutputLexeme> lexemes);       
    }
}
