using Domain.Entities;
using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Translator
{
    public interface IRPNExpressionCalculator
    {
        double Calculate(List<string> rpn, Dictionary<string,int> variables, List<OutputIdn> idns);

        List<RPNCalculatorTable> GetResultTable();
    }
}
