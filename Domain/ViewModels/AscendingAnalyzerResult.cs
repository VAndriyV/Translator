using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ViewModels
{
    public class AscendingAnalyzerResult
    {
        public LAResult LAResult { get; set; }

        public List<AscendingAnalyzerTable> AscendingAnalyzerTable{get;set;}
    }
}
