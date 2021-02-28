using System.Reflection;
using Autofac;
using FluentValidation;
using Library.API.Infrastructure.Behaviors;
using Library.Application.Commands.Books.CreateBookCommand;
using Library.Application.DomainEventHandlers;
using MediatR;

namespace Library.API.Infrastructure.AutofacModules
{
    public class MediatorModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Mediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();

            builder.Register<ServiceFactory>(context =>
            {
                var c = context.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            // Register CommandHandlers
            builder.RegisterAssemblyTypes(typeof(CreateBookCommand).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>));

            // Register DomainEventHandlers
            builder.RegisterAssemblyTypes(typeof(BookLoanFinishedEventHandler).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(INotificationHandler<>));

            // Register command's Validators
            builder.RegisterAssemblyTypes(typeof(CreateBookCommandValidator).GetTypeInfo().Assembly)
                .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
                .AsImplementedInterfaces();

            builder.RegisterGeneric(typeof(LoggingBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(ValidationBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(TransactionBehaviour<,>)).As(typeof(IPipelineBehavior<,>));
        }
    }
}