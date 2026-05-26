using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Proge.IntegrationTests.Helpers
{
    public class TestApplicationFactory<TTestStartup> : WebApplicationFactory<TTestStartup> where TTestStartup : class
    {
        protected override IHostBuilder CreateHostBuilder()
        {
            var host = Host.CreateDefaultBuilder()
                            .ConfigureWebHost(builder =>
                            {
                                builder.UseContentRoot(".");
                                builder.ConfigureAppConfiguration((c, b) =>
                                {
                                    c.HostingEnvironment.ApplicationName = "Proge2.1";
                                });
                                builder.UseStartup<TTestStartup>();

                                builder.ConfigureTestServices(services =>
                                {
                                    services.AddAntiforgery(options =>
                                    {
                                        options.Cookie.Name = "AntiforgeryCookie";
                                        options.HeaderName = "X-XSRF-TOKEN";
                                    });
                                });
                            })
                            .ConfigureAppConfiguration((context, conf) =>
                            {
                                var projectDir = Directory.GetCurrentDirectory();
                                var configPath = Path.Combine(projectDir, "appsettings.json");
                                conf.AddJsonFile(configPath);
                            });
            return host;
        }
    }
}