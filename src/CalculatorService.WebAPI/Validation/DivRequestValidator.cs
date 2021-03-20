using FluentValidation;
using System;
using System.Linq;
using CalculatorServices.WebAPI.DTOs;

namespace CalculatorServices.WebAPI.Validation
{
    public class DivRequestValidator : AbstractValidator<DivRequest>
    {
        public DivRequestValidator()
        {
            RuleFor(m => m.Divisor).GreaterThan(0).WithMessage(m => $"The divisor cannot be < 0 (param {m.Divisor})").WithErrorCode("1.1");
            RuleFor(m => m.Divisor).LessThan(1000).WithMessage(m => $"The divisor cannot be greater than 1000 (param {m.Divisor})").WithErrorCode("1.2");
            RuleFor(m => m.Dividend).LessThan(1000).WithMessage(m => $"he dividend cannot be greater than 1000 (param {m.Dividend})").WithErrorCode("1.3");
        }
    }
}
