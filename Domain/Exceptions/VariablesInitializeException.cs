using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions
{
    public class VariablesInitializeException : Exception
    {
        public IEnumerable<string> Variables { get; private set; }

        public VariablesInitializeException(IEnumerable<string> variables) : base()
        {          
            Variables = variables;
        }

        public VariablesInitializeException(string message, IEnumerable<string> variables) :base(message)
        {
            Variables = variables;
        }
    }
}
