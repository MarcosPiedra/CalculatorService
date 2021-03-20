﻿using CalculatorService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorService.Domain
{
    public interface IOperationService<P, R> : IOperationServiceBase where P : ParamsOperationBase where R : ResultOperationBase
    {
        Task<R> Execute(P parameters);
        string GetDescription(P parameters, R result);
    }
}
