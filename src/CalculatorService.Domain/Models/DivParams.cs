using System;
using System.Collections.Generic;
using System.Text;

namespace CalculatorService.Domain.Models
{
    public class DivParams : ParamsOperationBase
    {
        public int Dividend { get; set; }
        public int Divisor { get; set; }
    }
}
