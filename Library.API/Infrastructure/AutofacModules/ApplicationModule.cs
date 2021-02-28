using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using Autofac;
using Library.Application.Mappers;
using Library.Application.Queries.Books.GetBookDetailsQuery;
using Library.Application.Queries.Books.GetBookListQuery;
using Library.Application.Services;
using Library.Domain.AggregatesModel.BookAggregate;
using Library.Domain.AggregatesModel.BookLoanAggregate;
using Library.Domain.AggregatesModel.UserAggregate;
using Library.Domain.Services;
using Library.Infrastructure;
using Library.Infrastructure.Repositories;
using Library.Infrastructure.Services;
using Module = Autofac.Module;

namespace Library.API.Infrastructure.AutofacModules
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(BookMapper).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IMapper<,>))
                .AsImplementedInterfaces();

            builder.RegisterType<BookRepository>()
                .As<IBookRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<BookLoanRepository>()
                .As<IBookLoanRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<UserRepository>()
                .As<IUserRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<BookLoanService>().As<IBookLoanService>();
            builder.RegisterType<BookReturnService>().As<IBookReturnService>();

            builder.Register<ILibraryContext>(provider => provider.Resolve<LibraryContext>());

            builder.RegisterType<BookListMapper>().As<IBookListMapper>();
            builder.RegisterType<BookDetailsMapper>().As<IBookDetailsMapper>();
            
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<PasswordHashService>().As<IPasswordHashService>();
            builder.RegisterType<JwtTokenService>().As<IJwtTokenService>();

            builder.RegisterType<JwtSecurityTokenHandler>().AsSelf();
        }
    }
}