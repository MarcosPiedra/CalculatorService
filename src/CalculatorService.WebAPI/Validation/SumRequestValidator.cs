using FluentValidation;
using System;
using System.Linq;
using CalculatorServices.WebAPI.DTOs;

namespace CalculatorServices.WebAPI.Validation
{
    public class SumRequestValidator : AbstractValidator<SumRequest>
    {
        public SumRequestValidator()
        {
            RuleFor(m => m.Addends).Must(c => c == null || c.Count <= 9).WithMessage("The addends cannot be empty or greater than 9").WithErrorCode("2.1");
            RuleForEach(m => m.Addends).LessThanOrEqualTo(9).WithMessage("The addends cannot be greater than 9 (index {CollectionIndex})").WithErrorCode("2.2");
        }
    }
}
