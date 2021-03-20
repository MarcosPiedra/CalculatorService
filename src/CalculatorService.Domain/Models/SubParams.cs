using System;
using System.Collections.Generic;
using System.Text;

namespace CalculatorService.Domain.Models
{
    public class SubParams : ParamsOperationBase
    {
        public int Minuend { get; set; }
        public int Subtrahend { get; set; }
    }
}
