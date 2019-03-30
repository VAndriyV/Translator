using System;
using System.Collections.Generic;
using System.Text;

namespace TranslatorLogic.ReversePolishNotation.Configuration
{
    public class RPNPriorities
    {
        public static Dictionary<int,List<string>> GetPrioritiesConfiguration()
        {
            List<string> zeroPriority = new List<string>()
            {
                "(",
                "if",
                "for",
                "cout",
                "cin",
                "¶",
                "int",
                "float",
                "?"
            };

            List<string> onePriority = new List<string>()
            {
                "do",
                "then",
                "else",
                ")",
                "to",
                ":"
            };

            List<string> twoPriority = new List<string>()
            {
                "="
            };

            List<string> threePriority = new List<string>()
            {
                ">",
                "<",
                "==",
                "<=",
                ">=",
                "!="
            };

            List<string> fourPriority = new List<string>()
            {
                "+",
                "-"
            };

            List<string> fivePriority = new List<string>()
            {
                "*",
                "/"
            };

            List<string> sixPriority = new List<string>()
            {
                "^"
            };

            Dictionary<int, List<string>> priorities = new Dictionary<int, List<string>>
            {
                { 0, zeroPriority },
                { 1, onePriority },
                { 2, twoPriority },
                { 3, threePriority },
                { 4, fourPriority },
                { 5, fivePriority },
                { 6, sixPriority }
            };

            return priorities;            
        }
    }
}
