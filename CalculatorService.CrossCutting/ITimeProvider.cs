using System;

namespace CalculatorService.CrossCutting
{
    public interface ITimeProvider
    {
        DateTime GetNow();
    }
}