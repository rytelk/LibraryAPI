using System.Text;
using Autofac;
using Library.API.Infrastructure.AutofacModules;
using Library.API.Infrastructure.Extensions;
using Library.API.Infrastructure.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Converters;

namespace Library.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddLibraryDbContext(Configuration);
            services.AddCustomerSwagger();
            services.AddControllers()
                .AddNewtonsoftJson(opts => opts.SerializerSettings.Converters.Add(new StringEnumConverter()));
            services.AddJwtAuthentication(Configuration);
        }

        public virtual void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new MediatorModule());
            builder.RegisterModule(new ApplicationModule());
            builder.RegisterType<ErrorHandlingMiddleware>().InstancePerDependency();
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwagger()
                .UseSwaggerUI();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
