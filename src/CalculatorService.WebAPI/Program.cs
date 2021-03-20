using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace CalculatorService.WebAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

            try
            {
                logger.Info("Init Main");

                var host = Host.CreateDefaultBuilder(args)
                               .ConfigureWebHostDefaults(webHostBuilder =>
                               {
                                   webHostBuilder.ConfigureLogging(l => l.ClearProviders())
                                                 .ConfigureLogging(l => l.SetMinimumLevel(LogLevel.Trace))
                                                 .UseNLog()
                                                 .UseKestrel()
                                                 .UseStartup<Startup>()
                                                 .UseUrls("http://*:5001/");
                               })
                               .Build();

                await host.RunAsync();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }
    }
}
