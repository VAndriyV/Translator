using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions
{
    public class RPNException : Exception
    {
        public string ErrorDetails { get; }

        public RPNException(string message,string errorDetails):base(message)
        {
            ErrorDetails = errorDetails;
        }
    }
}
