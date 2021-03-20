using FluentValidation;
using System;
using System.Linq;
using CalculatorServices.WebAPI.DTOs;

namespace CalculatorServices.WebAPI.Validation
{
    public class SubRequestValidator : AbstractValidator<SubRequest>
    {
        public SubRequestValidator()
        {
            RuleFor(m => m.Minuend).LessThanOrEqualTo(10000).WithMessage("The minuend cannot be greather than 10000").WithErrorCode("4.1");
            RuleFor(m => m.Subtrahend).LessThanOrEqualTo(10000).WithMessage("The subtrahend cannot be greather than 10000").WithErrorCode("4.2");
        }
    }
}
