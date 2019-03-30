using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Translator
{
    public interface IRecursiveAnalyzer
    {
        void DoAnalyze(List<OutputLexeme> lexemes);
    }
}
