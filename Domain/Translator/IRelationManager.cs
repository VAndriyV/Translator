using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Translator
{
    public interface IRelationManager
    {
        void SetRelations(string grammarText);

        string[,] GetRelationMatrix();

        List<string> GetAllTerms();

        bool IsGrammarCorrect();

        Dictionary<string, List<List<string>>> GetGrammar();
    }
}
