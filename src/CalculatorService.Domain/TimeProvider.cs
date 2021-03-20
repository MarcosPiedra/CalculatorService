using System;
using System.Collections.Generic;
using System.Text;

namespace CalculatorService.Domain
{
    // This implementation / interface belongs to CrossCutting concerns
    public class TimeProvider : ITimeProvider
    {
        public DateTime GetNow() => DateTime.Now;
    }
}
