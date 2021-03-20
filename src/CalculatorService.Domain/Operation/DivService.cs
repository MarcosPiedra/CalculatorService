using CalculatorService.Domain.Models;
using System;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorService.Domain.Operation
{
    public class DivService : IOperationService<DivParams, DivResult>
    {
        public Task<DivResult> Execute(DivParams parameters)
        {
            return Task.FromResult(new DivResult
            {
                Quotient = Math.DivRem(parameters.Dividend, parameters.Divisor, out int remainder),
                Remainder = remainder
            });
        }

        public string GetDescription(DivParams parameters, DivResult result)
        {
            return new StringBuilder()
                .Append($"{parameters.Dividend} / {parameters.Divisor} = {result.Quotient} , ")
                .Append($"{ parameters.Dividend} % { parameters.Divisor} = { result.Quotient}")
                .ToString();
        }
    }
}