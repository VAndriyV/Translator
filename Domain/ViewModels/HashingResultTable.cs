using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ViewModels
{
    public class HashingResultTable
    {
        public List<double> LinearStatistic { get; set; }

        public List<double> SquareStatistic { get; set; }

        public List<double> FakeSquareStatistic { get; set; }

        public List<double> FakeLinearStatistic { get; set; }
    }
}
