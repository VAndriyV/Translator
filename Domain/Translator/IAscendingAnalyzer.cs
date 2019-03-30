using Domain.Entities;
using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Translator
{
    public interface IAscendingAnalyzer
    {
        Task<List<AscendingAnalyzerTable>> DoSyntaxAnalyze(Dictionary<string, List<List<string>>> grammar
            , List<string> allTerms
            , int[,] relationMatrix
            , List<OutputLexeme> inputLexemes);       
    }
}
