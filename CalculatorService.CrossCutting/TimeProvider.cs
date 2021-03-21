using System;

namespace CalculatorService.CrossCutting
{
    public class TimeProvider : ITimeProvider
    {
        public DateTime GetNow() => DateTime.Now;
    }
}
