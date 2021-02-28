using System;
using System.Threading;
using System.Threading.Tasks;
using Library.API.Infrastructure.Extensions;
using Library.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Library.API.Infrastructure.Behaviors
{
    public class TransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<TransactionBehaviour<TRequest, TResponse>> _logger;
        private readonly LibraryContext _dbContext;

        public TransactionBehaviour(LibraryContext dbContext, ILogger<TransactionBehaviour<TRequest, TResponse>> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            var response = default(TResponse);
            var typeName = request.GetTypeName();

            try
            {
                if (_dbContext.HasActiveTransaction)
                {
                    return await next();
                }

                var strategy = _dbContext.Database.CreateExecutionStrategy();

                await strategy.ExecuteAsync(async () =>
                {
                    using (var transaction = await _dbContext.BeginTransactionAsync())
                    using (LogContext.PushProperty("TransactionContext", transaction.TransactionId))
                    {
                        _logger.LogInformation("Begin transaction {TransactionId} for {CommandName} ({@Command})",
                            transaction.TransactionId, typeName, request);

                        response = await next();
                        await _dbContext.CommitTransactionAsync(transaction);

                        _logger.LogInformation("Commit transaction {TransactionId} for {CommandName}",
                            transaction.TransactionId, typeName);
                    }
                });

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Handling transaction for {CommandName} ({@Command})", typeName, request);
                throw;
            }
        }
    }
}