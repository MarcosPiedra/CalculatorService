using CalculatorService.Domain.Models;
using System;
using System.Threading.Tasks;

namespace CalculatorService.Domain.Operation
{
    public class SqrtService : IOperationService<SqrtParams, IntResult>
    {
        public Task<IntResult> Execute(SqrtParams parameters) => Task.FromResult(new IntResult { Result = Convert.ToInt32(Math.Sqrt(parameters.Number)) });

        public string GetDescription(SqrtParams parameters, IntResult intResult) => $"Sqrt({parameters.Number}) = {intResult.Result}";
    }
}