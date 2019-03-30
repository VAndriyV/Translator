using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ViewModels
{
    public class RPNBuilderResult
    {
        public LAResult LaResult { get; set; }

        public RPNBuilderTable RPNBuilderTable { get; set; }

        public string SingleLineRPN { get; set; }
    }
}
