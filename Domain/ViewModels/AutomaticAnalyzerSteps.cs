using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ViewModels
{
    public class AutomaticAnalyzerSteps
    {
        public List<int> Numbers { get; set; } = new List<int>();
        public List<string> Lexemes { get; set; } = new List<string>();
        public List<List<int>> States { get; set; } = new List<List<int>>();
        public List<Stack<int>> Stack { get; set; } = new List<Stack<int>>();
    }
}
