using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ViewModels
{
    public class RPNExpressionBuilderResult
    {
        public List<RPNExpressionTable> ResultTable { get; set; }

        public List<OutputIdn> Idns { get; set; }
    }
}
