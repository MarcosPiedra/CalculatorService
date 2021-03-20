using CalculatorService.Domain.Models;

namespace CalculatorService.Domain
{
    public static class OperationServiceExtensions
    {
        public static string GetServiceName<P, O>(this IOperationService<P, O> service) where P : ParamsOperationBase where O : ResultOperationBase
        {
            return service.GetType().Name.Replace("Service", "");
        }
    }
}
