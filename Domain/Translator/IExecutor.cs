using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Translator
{
    public interface IExecutor
    {
        string Execute(Dictionary<string, int> marks, List<string> RPN, List<OutputIdn> IDNs
            , Dictionary<string, long> additinalCells);
    }
}
