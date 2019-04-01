using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ViewModels
{
    public class ExecutorTable
    {
        public List<List<string>> Stack { get; set; } = new List<List<string>>();

        public List<List<string>> RPN { get; set; } = new List<List<string>>();

        public List<string> Description { get; set; } = new List<string>();
    }
}
