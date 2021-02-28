using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Library.API.Infrastructure.Exceptions.CommandValidationException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Library.API.Infrastructure.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;

        public ValidationBehavior(IServiceProvider serviceProvider, ILogger<ValidationBehavior<TRequest, TResponse>> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var validator = (IValidator<TRequest>)_serviceProvider.GetService(typeof(IValidator<TRequest>));
            if (validator != null)
            {
                _logger.LogInformation("Validating command {CommandType}", request.GetType().Name);

                var failures = (await validator.ValidateAsync(request, cancellationToken)).Errors;
                if (failures.Any())
                {
                    _logger.LogWarning("Validation errors - {CommandType} - Command: {@Command} - Errors: {@ValidationErrors}", 
                        request.GetType().Name, request, failures);

                    var validationErrors = failures.Select(er => new ValidationError
                    {
                        FieldName = er.PropertyName,
                        Message = er.ErrorMessage,
                        AttemptedValue = er.AttemptedValue,
                    }).ToList();
                    throw new CommandValidationException(validationErrors);
                }
            }

            return await next();
        }
    }
}