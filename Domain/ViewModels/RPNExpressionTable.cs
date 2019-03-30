using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ViewModels
{
    public class RPNExpressionTable
    {
        public List<string> Stack { get; set; }

        public List<string> InputTokens { get; set; }

        public string Relation { get; set; }

        public List<string> RPN { get; set; }

        
    }
}
