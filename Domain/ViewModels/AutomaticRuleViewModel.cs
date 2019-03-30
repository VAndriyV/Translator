using System;
using System.Collections.Generic;

namespace Domain.ViewModels
{
    public class AutomaticRuleViewModel
    {
        public int Id { get; set; }
        public int Alpha { get; set; }
        public string Lexeme { get; set; }
        public int? Beta { get; set; }
        public int? Stack { get; set; }
        public string Information { get; set; }
    }
}
