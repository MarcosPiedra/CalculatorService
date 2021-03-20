using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalculatorService.WebAPI.DTOs
{
    public class ErrorResponse
    {
        public ErrorResponse(string code, string message, string status)
        {
            Code = code;
            Message = message;
            Status = status;
        }

        public ErrorResponse(string code, string message, string status, IList<ValidationFailure> errors)
        {
            Code = code;
            Message = message;
            Status = status;
            Errors = new List<SingleErrorResponse>();
            foreach (var e in errors)
            {
                Errors.Add(new SingleErrorResponse()
                {
                    Code = e.ErrorCode,
                    Message = e.ErrorMessage
                });
            }
        }

        public string Code { get; set; }
        public string Message { get; set; }
        public List<SingleErrorResponse> Errors { get; set; }
        public string Status { get; internal set; }
    }

    public class SingleErrorResponse
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }
}
