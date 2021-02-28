using System;
using Library.API;
using Library.Domain.AggregatesModel.UserAggregate;
using Library.Infrastructure;
using Library.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Library.FunctionalTests.Infrastructure
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            base.Configure(app, env);

            var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using var serviceScope = serviceScopeFactory.CreateScope();
            
            var libraryContext = serviceScope.ServiceProvider.GetService<LibraryContext>();
            if(libraryContext == null)
            {
                throw new NullReferenceException("Cannot get instance of dbContext");
            }
            
            libraryContext.Database.EnsureDeleted();
            libraryContext.Database.EnsureCreated();

            var passwordHashService = serviceScope.ServiceProvider.GetService<IPasswordHashService>();
            var (passwordHash, passwordSalt) = passwordHashService.CreatePasswordHash("password");
            libraryContext.Users.Add(new User("AdminFirstName", "AdminLastName", new Email("admin@gmail.com"), 
                passwordHash, passwordSalt, UserRolesConsts.Librarian));

            libraryContext.SaveChanges();
        }
    }
}