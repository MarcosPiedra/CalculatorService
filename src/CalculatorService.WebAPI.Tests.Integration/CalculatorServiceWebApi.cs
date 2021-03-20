using Microsoft.AspNetCore.Hosting;
using System;
using Microsoft.AspNetCore.TestHost;
using System.Reflection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using CalculatorService.WebAPI;

namespace CalculatorServices.WebAPI.Tests.Unit
{
    public class CalculatorServiceWebApi
    {
        public async Task<IHost> GetServer(Action<IServiceCollection> servicesConfiguration = null)
        {
            var path = Assembly.GetAssembly(typeof(CalculatorServiceWebApi)).Location;
            var hostBuilder = new HostBuilder()
                        .ConfigureWebHost(webHost =>
                        {
                            webHost.UseTestServer();
                            webHost.UseStartup<Startup>();
                        });

            return await hostBuilder.StartAsync();
        }
    }
}
