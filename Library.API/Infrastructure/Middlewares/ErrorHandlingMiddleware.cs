using System;
using System.Net;
using System.Threading.Tasks;
using Library.API.Infrastructure.Exceptions.CommandValidationException;
using Library.API.Infrastructure.Extensions;
using Library.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Library.API.Infrastructure.Middlewares
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (CommandValidationException ex)
            {
                _logger.LogError(ex, "Command validation errors occured.");
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";
                var response = new ValidationErrorResponse
                {
                    Errors = ex.ValidationErrors
                };
                await context.Response.WriteAsync(response.SerializeToJson(true));
            }
            catch (DomainObjectNotFound ex)
            {
                _logger.LogError(ex, "Domain object not found exception.");
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                context.Response.ContentType = "text/plain; charset=utf-8";
                await context.Response.WriteAsync(ex.Message);
            }
            catch (LibraryDomainException ex)
            {
                _logger.LogError(ex, "Library domain exception.");
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "text/plain; charset=utf-8";
                await context.Response.WriteAsync(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception.");
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                context.Response.ContentType = "text/plain; charset=utf-8";
                await context.Response.WriteAsync(ex.Message);
            }
        }
    }
}