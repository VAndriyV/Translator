using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ViewModels
{
    public class AscendingAnalyzerTable
    {
        public List<string> Stack { get; set; }

        public string Relation { get; set; }

        public List<string> Lexemes { get; set; }
       
        public AscendingAnalyzerTable(List<string> stack, List<string> lexems, int relation)
        {
            Stack = stack;

            Lexemes = lexems;

            if (relation == 1)
                Relation = "<";
            else if (relation == 2)
                Relation = "=";
            else if (relation == 3)
                Relation = ">";
        }   
    }
}
