using Domain.Entities;
using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Translator
{
    public interface IRPNExpressionBuilder
    {
        void BuildRPN(Dictionary<string, List<List<string>>> grammar
            , List<string> allTerms
            , int[,] relationMatrix
            , List<OutputLexeme> inputLexemes);

        List<RPNExpressionTable> GetResultTable();

        List<OutputIdn> GetOutputIdns();

        List<string> GetRPN();
    }
}
