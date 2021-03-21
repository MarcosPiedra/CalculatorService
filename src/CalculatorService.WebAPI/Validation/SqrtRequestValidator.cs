using FluentValidation;
using System;
using System.Linq;
using CalculatorServices.WebAPI.DTOs;

namespace CalculatorServices.WebAPI.Validation
{
    public class SqrtRequestValidator : AbstractValidator<SqrtRequest>
    {
        public SqrtRequestValidator()
        {
            RuleFor(m => m.Number).LessThanOrEqualTo(10000).WithMessage("The number cannot be greater than 10000").WithErrorCode("3.1");
        }
    }
}
