using CalculatorService.Domain.Models;
using System.Threading.Tasks;

namespace CalculatorService.Domain.Operation
{
    public class SubService : IOperationService<SubParams, IntResult>
    {
        public Task<IntResult> Execute(SubParams parameters) => Task.FromResult(new IntResult { Result = parameters.Minuend - parameters.Subtrahend });

        public string GetDescription(SubParams parameters, IntResult intResult) => $"{parameters.Minuend} - {parameters.Subtrahend} = {intResult.Result}";
    }
}