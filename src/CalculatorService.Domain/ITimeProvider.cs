using System;

namespace CalculatorService.Domain
{
    public interface ITimeProvider
    {
        DateTime GetNow();
    }
}