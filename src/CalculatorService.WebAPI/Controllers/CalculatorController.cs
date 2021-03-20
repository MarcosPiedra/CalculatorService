using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using CalculatorService.Domain;
using CalculatorService.Domain.Models;
using CalculatorService.Domain.Operation;
using CalculatorService.WebAPI.DTOs;
using CalculatorServices.WebAPI.DTOs;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CalculatorServices.WebAPI.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[Controller]")]
    public class CalculatorController : Controller
    {
        private readonly ILogger<CalculatorController> logger;
        private readonly IMapper mapper;
        private readonly Func<string, IOperationServiceBase> getOperation;
        private readonly IRepository<Operation> repository;
        private readonly IValidator<DivRequest> divValidator;
        private readonly IValidator<MultRequest> multValidator;
        private readonly IValidator<SqrtRequest> sqrtValidator;
        private readonly IValidator<SubRequest> subValidator;
        private readonly IValidator<SumRequest> sumValidator;

        public CalculatorController(ILogger<CalculatorController> logger,
                                    Func<string, IOperationServiceBase> getOperation,
                                    IRepository<Operation> repository,
                                    IMapper mapper,
                                    IValidator<DivRequest> divValidator,
                                    IValidator<MultRequest> multValidator,
                                    IValidator<SqrtRequest> sqrtValidator,
                                    IValidator<SubRequest> subValidator,
                                    IValidator<SumRequest> sumValidator)
        {
            this.logger = logger;
            this.getOperation = getOperation;
            this.repository = repository;
            this.mapper = mapper;
            this.divValidator = divValidator;
            this.multValidator = multValidator;
            this.sqrtValidator = sqrtValidator;
            this.subValidator = subValidator;
            this.sumValidator = sumValidator;
        }

        [HttpPost("div")]
        [ProducesResponseType(typeof(DivResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<DivResponse>> PostDiv([FromBody] DivRequest request)
        {
            var validate = divValidator.Validate(request);
            if (!validate.IsValid)
                return BadRequest(new ErrorResponse("BadRequest", "Unable to process request", "400", validate.Errors));

            var parameters = mapper.Map<DivParams>(request);
            var operation = GetOp<DivParams, DivResult>(nameof(DivService));
            var result = await operation.Execute(parameters);

            logger.LogInformation($"PostDiv executed");

            var resultResponse = this.mapper.Map<DivResponse>(result);
            return resultResponse;
        }

        [HttpPost("mult")]
        [ProducesResponseType(typeof(MultResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<MultResponse>> PostMult([FromBody] MultRequest request)
        {
            var validate = multValidator.Validate(request);
            if (!validate.IsValid)
                return BadRequest(new ErrorResponse("BadRequest", "Unable to process request", "400", validate.Errors));

            var parameters = mapper.Map<MultParams>(request);
            var operation = GetOp<MultParams, IntResult>(nameof(MultService));
            var result = await operation.Execute(parameters);

            logger.LogInformation($"PostMult executed");

            var resultResponse = this.mapper.Map<MultResponse>(result);
            return resultResponse;
        }

        [HttpPost("sqrt")]
        [ProducesResponseType(typeof(SqrtResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<SqrtResponse>> PostSqrt([FromBody] SqrtRequest request)
        {
            var validate = sqrtValidator.Validate(request);
            if (!validate.IsValid)
                return BadRequest(new ErrorResponse("BadRequest", "Unable to process request", "400", validate.Errors));

            var parameters = mapper.Map<SqrtParams>(request);
            var operation = GetOp<SqrtParams, IntResult>(nameof(SqrtService));
            var result = await operation.Execute(parameters);

            logger.LogInformation($"PostSqrt executed");

            var resultResponse = this.mapper.Map<SqrtResponse>(result);
            return resultResponse;
        }

        [HttpPost("sub")]
        [ProducesResponseType(typeof(SubResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<SubResponse>> PostSub([FromBody] SubRequest request)
        {
            var validate = subValidator.Validate(request);
            if (!validate.IsValid)
                return BadRequest(new ErrorResponse("BadRequest", "Unable to process request", "400", validate.Errors));

            var parameters = mapper.Map<SubParams>(request);
            var operation = GetOp<SubParams, IntResult>(nameof(SubService));
            var result = await operation.Execute(parameters);

            logger.LogInformation($"PostSub executed");

            var resultResponse = this.mapper.Map<SubResponse>(result);
            return resultResponse;
        }

        [HttpPost("add")]
        [ProducesResponseType(typeof(SumResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<SumResponse>> PostSum([FromBody] SumRequest request)
        {
            var validate = sumValidator.Validate(request);
            if (!validate.IsValid)
                return BadRequest(new ErrorResponse("BadRequest", "Unable to process request", "400", validate.Errors));

            var parameters = mapper.Map<SumParams>(request);
            var operation = GetOp<SumParams, IntResult>(nameof(SumService));
            var result = await operation.Execute(parameters);

            logger.LogInformation($"PostSum executed");

            var resultResponse = this.mapper.Map<SumResponse>(result);
            return resultResponse;
        }

        [HttpPost("query")]
        [ProducesResponseType(typeof(QueryResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<QueryResponse>> PostQuery([FromBody] QueryRequest request)
        {
            var operations = await repository.Query.OrderBy(o => o.DateTime).Where(q => q.TrackId == request.Id).ToListAsync();

            logger.LogInformation($"PostQuery executed");

            return new QueryResponse()
            {
                Operations = this.mapper.Map<List<OperationResponse>>(operations)
            };
        }

        [HttpGet("operations")]
        [ProducesResponseType(typeof(QueryResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<QueryResponse>> GetAllOperations()
        {
            var operations = await repository.Query.OrderBy(o => o.DateTime).ToListAsync();

            logger.LogInformation($"GetAllOperations executed");

            return new QueryResponse()
            {
                Operations = this.mapper.Map<List<OperationResponse>>(operations)
            };
        }

        public IOperationService<P, R> GetOp<P, R>(string parameterTypeNamed) where P : ParamsOperationBase where R : ResultOperationBase
        {
            return getOperation.Invoke(parameterTypeNamed) as IOperationService<P, R>;
        }
    }
}