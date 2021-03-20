using CalculatorService.Domain.Models;
using System.Linq;
using System.Threading.Tasks;

namespace CalculatorService.Domain.Operation
{
    public class SumService : IOperationService<SumParams, IntResult>
    {
        public Task<IntResult> Execute(SumParams parameters) => Task.FromResult(new IntResult() { Result = parameters.Addends.Sum() });

        public string GetDescription(SumParams parameters, IntResult intResult) => $"{string.Join(" + ", parameters.Addends)} = {intResult.Result}";
    }
}