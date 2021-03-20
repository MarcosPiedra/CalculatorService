using CalculatorService.Domain.Models;
using OperationModel = CalculatorService.Domain.Models.Operation;
using System.Threading.Tasks;

namespace CalculatorService.Domain
{
    public class ServiceTracked<P, R> : IOperationService<P, R> where P : ParamsOperationBase where R : ResultOperationBase
    {
        private readonly ITimeProvider timeProvider;
        private readonly IOperationService<P, R> innerOperation;
        private readonly IRepository<OperationModel> repository;
        private readonly int trackId;

        public ServiceTracked(IOperationService<P, R> innerOperation,
                              int trackId,
                              ITimeProvider timeProvider,
                              IRepository<OperationModel> repository)
        {
            this.timeProvider = timeProvider;
            this.innerOperation = innerOperation;
            this.trackId = trackId;
            this.repository = repository;
        }

        public Task<R> Execute(P parameters)
        {
            var task = innerOperation
                         .Execute(parameters)
                         .ContinueWith(async (a) =>
                         {
                             var result = a.Result;
                             var operation = new OperationModel()
                             {
                                 TrackId = trackId,
                                 OperationType = innerOperation.GetServiceName(),
                                 Calculation = innerOperation.GetDescription(parameters, result),                                 
                                 DateTime = this.timeProvider.GetNow(),
                             };
                             await repository.Add(operation);
                             await repository.Save();
                             return result;
                         }, TaskContinuationOptions.OnlyOnRanToCompletion)
                         .Unwrap();

            return task;
        }

        public string GetDescription(P parameters, R result) => string.Empty;
    }
}