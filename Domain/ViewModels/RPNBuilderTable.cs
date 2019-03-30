using System.Collections.Generic;

namespace Domain.ViewModels
{
    public class RPNBuilderTable
    {
        public List<string> InputLexemes { get; set; } = new List<string>();

        public List<List<string>> Stack { get; set; } = new List<List<string>>();

        public List<List<string>> RPN { get; set; } = new List<List<string>>();

        public List<string> LoopArgument { get; set; } = new List<string>();

        public List<char> LoopIndicator { get; set; } = new List<char>();
    }
}
