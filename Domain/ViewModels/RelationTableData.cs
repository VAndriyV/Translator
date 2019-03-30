using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ViewModels
{
    public class RelationTableData
    {
        public List<string> AllTerms { get; set; }

        public string [,] RelationMatrix { get; set; } 
    }
}
