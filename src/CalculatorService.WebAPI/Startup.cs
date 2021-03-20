using System;
using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using FluentValidation;
using CalculatorServices.WebAPI.DTOs;
using CalculatorServices.WebAPI.Validation;
using CalculatorServices.Configurations;
using CalculatorService.SqlDataAccess;
using CalculatorService.Domain;
using CalculatorService.Domain.Models;
using CalculatorService.Domain.Operation;
using Microsoft.Extensions.Primitives;
using CalculatorService.WebAPI.DTOs;
using Newtonsoft.Json;

namespace CalculatorService.WebAPI
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        private string currentBin;

        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                         .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                         .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                         .AddEnvironmentVariables();

            currentBin = System.IO.Directory.GetParent(typeof(Program).Assembly.Location).FullName;
            configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var connConfig = configuration.GetSection("ConnectionStrings").Get<ConnectionConfiguration>();

            services.AddMvc()
                    .AddFluentValidation()
                    .AddNewtonsoftJson();

            services.AddAutoMapper(typeof(Startup));

            services.AddControllers();
            services.AddApiVersioning();

            services.AddDbContext<OperationContext>((serviceProvider, optionsBuilder) =>
            {
                var dbSourceText = "Data Source=";
                var dbName = connConfig.DatabaseConnection
                                .Replace(dbSourceText, "", StringComparison.OrdinalIgnoreCase);
                var binPath = System.IO.Path.Combine(currentBin, dbName);
                optionsBuilder.UseSqlite($"{dbSourceText}{binPath}");
            }, ServiceLifetime.Transient);

            services.AddTransient<IValidator<DivRequest>, DivRequestValidator>();
            services.AddTransient<IValidator<MultRequest>, MultRequestValidator>();
            services.AddTransient<IValidator<SqrtRequest>, SqrtRequestValidator>();
            services.AddTransient<IValidator<SubRequest>, SubRequestValidator>();
            services.AddTransient<IValidator<SumRequest>, SumRequestValidator>();

            services.AddTransient<IRepository<Operation>, EFRepository<Operation>>();
            services.AddTransient<ITimeProvider, TimeProvider>();

            services.AddTransient<IOperationService<DivParams, DivResult>, DivService>();
            services.AddTransient<IOperationService<MultParams, IntResult>, MultService>();
            services.AddTransient<IOperationService<SqrtParams, IntResult>, SqrtService>();
            services.AddTransient<IOperationService<SubParams, IntResult>, SubService>();
            services.AddTransient<IOperationService<SumParams, IntResult>, SumService>();

            services.AddTransient<Func<string, IOperationServiceBase>>(provider =>
            {
                return (string parameterTypeNamed) =>
                {
                    var context = provider.GetRequiredService<IHttpContextAccessor>();
                    var trackId = 0;
                    var toTrack = context.HttpContext?.Request.Headers.ContainsKey("X-Evi-Tracking-Id") == true;
                    if (toTrack)
                    {
                        context.HttpContext?.Request.Headers.TryGetValue("X-Evi-Tracking-Id", out StringValues trackIdvalue);
                        toTrack = int.TryParse(trackIdvalue, out trackId);
                    }

                    IOperationServiceBase op = parameterTypeNamed switch
                    {
                        nameof(DivService) => Get<DivParams, DivResult>(provider, toTrack, trackId),
                        nameof(MultService) => Get<MultParams, IntResult>(provider, toTrack, trackId),
                        nameof(SqrtService) => Get<SqrtParams, IntResult>(provider, toTrack, trackId),
                        nameof(SubService) => Get<SubParams, IntResult>(provider, toTrack, trackId),
                        nameof(SumService) => Get<SumParams, IntResult>(provider, toTrack, trackId),
                        _ => null,
                    };

                    return op;
                };
            });
        }

        private IOperationServiceBase Get<P, R>(IServiceProvider provider, bool toTrack, int trackId) where P : ParamsOperationBase where R : ResultOperationBase
        {
            var op = provider.GetRequiredService<IOperationService<P, R>>();

            if (toTrack)
            {
                var repo = provider.GetRequiredService<IRepository<Operation>>();
                var time = provider.GetRequiredService<ITimeProvider>();
                op = new ServiceTracked<P, R>(op, trackId, time, repo);
            }

            return op;
        }

        public void Configure(IApplicationBuilder app,
                              IWebHostEnvironment env,
                              ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    string message = $"{context.Request.Path} {context.Request.QueryString} {context.Request.Method}";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        logger.Log(LogLevel.Error, contextFeature.Error, message);
                        var errorResponse = new ErrorResponse("Internal error", contextFeature.Error.Message, context.Response.StatusCode.ToString());
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
                    }
                });
            });

            app.UseRouting();
            app.UseEndpoints(e => e.MapControllers());
        }
    }
}
