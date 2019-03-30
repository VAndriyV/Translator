using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ViewModels
{
    public class RPNCalculatorTable
    {
        public List<string> Stack { get; set; }

        public List<string> RPN { get; set; }
    }
}
