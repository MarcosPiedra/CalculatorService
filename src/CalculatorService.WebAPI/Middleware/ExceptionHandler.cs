using CalculatorService.WebAPI.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace CalculatorService.WebAPI.Middleware
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionHandler> logger;

        public ExceptionHandler(RequestDelegate next, ILogger<ExceptionHandler> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionMessageAsync(context, ex).ConfigureAwait(false);
            }
        }

        private Task HandleExceptionMessageAsync(HttpContext context, Exception exception)
        {
            var message = $"{context.Request.Path} {context.Request.QueryString} {context.Request.Method}";
            logger.LogError(exception, $"Internal server error: {message}");

            context.Response.ContentType = "application/json";            
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var errorResponse = new ErrorResponse("Internal error", exception.Message, context.Response.StatusCode.ToString());
            return context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
        }
    }
}
