using FluentValidation;
using System;
using System.Linq;
using CalculatorServices.WebAPI.DTOs;

namespace CalculatorServices.WebAPI.Validation
{
    public class MultRequestValidator : AbstractValidator<MultRequest>
    {
        public MultRequestValidator()
        {
            RuleFor(m => m.Factors).Must(c => c == null || c.Count <= 9).WithMessage("The number of factors cannot be empty or greather than 9").WithErrorCode("2.1");
            RuleForEach(m => m.Factors).LessThanOrEqualTo(9).WithMessage("The number of factor cannot be greather than 9 (index {CollectionIndex})").WithErrorCode("2.2");
        }
    }
}
