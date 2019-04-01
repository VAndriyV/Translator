using Domain.Entities;
using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Translator
{
    public interface IExecutor
    {
        string Execute(Dictionary<string, int> marks, List<string> RPN, List<OutputIdn> IDNs
            , Dictionary<string, long> additinalCells);

        string ContinueExecution(Dictionary<string, long> values, int idx);

        int GetLastIdx();

        ExecutorTable GetResultTable();
    }
}
