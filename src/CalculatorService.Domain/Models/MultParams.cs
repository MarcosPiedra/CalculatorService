using System;
using System.Collections.Generic;
using System.Text;

namespace CalculatorService.Domain.Models
{
    public class MultParams : ParamsOperationBase
    {
        public List<int> Factors { get; set; }
    }
}
