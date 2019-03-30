using System;

namespace Domain.Exceptions
{
    public class LexicalAnalyzeException : Exception
    {
        public string AnalyzeErrors { get; }

        public LexicalAnalyzeException(string message, string errors) : base(message)
        {
            AnalyzeErrors = errors;
        }
        public LexicalAnalyzeException(string message) : base(message)
        {

        }

    }
}
