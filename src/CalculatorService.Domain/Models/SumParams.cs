using System;
using System.Collections.Generic;
using System.Text;

namespace CalculatorService.Domain.Models
{
    public class SumParams : ParamsOperationBase
    {
        public List<int> Addends { get; set; }
    }
}
