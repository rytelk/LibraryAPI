using System.IO;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Library.FunctionalTests.Infrastructure
{
    public class TestApplicationFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint> where TEntryPoint : class
    {
        protected override IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder(null)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHost(op =>
                {
                    op.UseStartup<TEntryPoint>();
                })
                .ConfigureAppConfiguration((context, conf) =>
                {
                    var projectDir = Directory.GetCurrentDirectory();
                    var configPath = Path.Combine(projectDir, "appsettings.json");
                    conf.AddJsonFile(configPath);
                });
        }
    }
}