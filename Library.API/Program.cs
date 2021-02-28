using System;
using Autofac.Extensions.DependencyInjection;
using Library.API.Infrastructure.Extensions;
using Library.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Core;

namespace Library.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = CreateSerilogLogger();

            try
            {
                Log.Information("Building web host...");
                var host = CreateHostBuilder(args).Build();

                Log.Information("Applying database migrations...");
                host.MigrateDbContext<LibraryContext>((context, services) =>
                {
                    new LibraryContextSeed()
                        .SeedAsync(context, services)
                        .Wait();
                });

                Log.Information("Starting web host...");
                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Program terminated unexpectedly.");
            }
            finally
            {
                Log.CloseAndFlush();
            }

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseSerilog();

        private static Logger CreateSerilogLogger()
        {
            return new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", Serilog.Events.LogEventLevel.Warning)
                .WriteTo.Console()
                .WriteTo.File("logs.txt",
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true)
                .CreateLogger();
        }
    }
}
