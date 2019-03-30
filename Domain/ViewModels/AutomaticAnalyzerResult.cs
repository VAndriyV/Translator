using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ViewModels
{
    public class AutomaticAnalyzerResult
    {
        public LAResult LAResult { get; set; }

        public AutomaticAnalyzerSteps AutomaticSteps { get; set; }
    }
}
