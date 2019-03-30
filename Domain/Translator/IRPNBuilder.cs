using Domain.Entities;
using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Translator
{
    public interface IRPNBuilder
    {
        void BuildRPN(List<OutputLexeme> inputLexemes);

        List<string> GetRPN();

        RPNBuilderTable GetResultTable();
    }
}
