using System;
using System.Collections.Generic;

namespace Domain.ViewModels
{
    public class OutputLexemeViewModel
    {
        public int Id { get; set; }
        public int Row { get; set; }
        public string LexemeName { get; set; }
        public int LexemeId { get; set; }
        public int? Idncode { get; set; }
        public int? ConstantCode { get; set; }
    }
}
