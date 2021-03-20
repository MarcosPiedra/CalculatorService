using CalculatorService.Domain.Models;
using System.Linq;
using System.Threading.Tasks;

namespace CalculatorService.Domain.Operation
{
    public class MultService : IOperationService<MultParams, IntResult>
    {
        public Task<IntResult> Execute(MultParams parameters) => Task.FromResult(new IntResult { Result = parameters.Factors.Aggregate((r, f) => r * f) });

        public string GetDescription(MultParams parameters, IntResult intResult) => $"{string.Join(" * ", parameters.Factors)} = {intResult.Result}";
    }
}