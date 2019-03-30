using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions
{
    public class SyntaxAnalyzeException : Exception
    {
        public string AnalyzeErrors { get; }

        public SyntaxAnalyzeException(string message, string errors) : base(message)
        {
            AnalyzeErrors = errors;
        }

        public SyntaxAnalyzeException(string message) : base(message)
        {

        }
    }
}
