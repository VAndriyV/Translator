using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ViewModels
{
    public class LAResult
    {
        public List<OutputConstantViewModel> OutputConstants { get; set; }
        public List<OutputIdnViewModel> OutputIdns { get; set; }
        public List<OutputLexemeViewModel> OutputLexemes { get; set; }
    }
}
