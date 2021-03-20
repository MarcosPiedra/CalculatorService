using System;
using System.Collections.Generic;
using System.Text;

namespace CalculatorService.Domain.Models
{
    public class DivResult : ResultOperationBase
    {
        public int Quotient { get; set; }
        public int Remainder { get; set; }
    }
}
