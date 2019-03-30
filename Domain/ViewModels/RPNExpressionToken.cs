using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ViewModels
{
    public class RPNExpressionToken
    {
        public string Name { get; set; }
        
        public string Type { get; set; }

        public RPNExpressionToken(string name,string type)
        {
            Name = name;
            Type = type;
        }
    }
}
