using System;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;

namespace Library.API.Infrastructure.Extensions
{
    public static class WebHostExtensions
    {
        public static IHost MigrateDbContext<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder) where TContext : DbContext
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<TContext>>();
            var context = services.GetService<TContext>();

            try
            {
                logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext).Name);

                var retries = 15;
                var retry = Policy.Handle<SqlException>()
                    .WaitAndRetry(retries, retryAttempt => TimeSpan.FromSeconds(10),
                        (exception, timeSpan, retry, ctx) =>
                        {
                            logger.LogInformation("Migration of database retry {retry} of {retries}...", retry, retries);
                        });

                // wait for SQL Server docker container to become available
                retry.Execute(() => InvokeSeeder(seeder, context, services));

                logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TContext).Name);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(TContext).Name);
            }

            return host;
        }

        private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider serviceProvider)
            where TContext : DbContext
        {
            context.Database.Migrate();
            seeder(context, serviceProvider);
        }
    }
}
