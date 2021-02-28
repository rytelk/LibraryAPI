using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Library.Domain.AggregatesModel.BookAggregate;
using Library.Domain.AggregatesModel.BookLoanAggregate;
using Library.Domain.AggregatesModel.UserAggregate;
using Library.Infrastructure.EntityConfigurations;
using Library.Infrastructure.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Library.Infrastructure
{
    public class LibraryContext : DbContext, ILibraryContext
    {
        public const string DefaultSchema = "Library";

        private readonly IMediator _mediator;

        public DbSet<Book> Books { get; set; }
        public DbSet<BookLoan> BookLoans { get; set; }
        public DbSet<User> Users { get; set; }


        private IDbContextTransaction _currentTransaction;

        public bool HasActiveTransaction => _currentTransaction != null;

        public LibraryContext(DbContextOptions<LibraryContext> options) :
            base(options)
        {
        }

        public LibraryContext(DbContextOptions<LibraryContext> options, IMediator mediator) :
            base(options)
        {
            _mediator = mediator;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BookEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new BookLoanEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _mediator.DispatchDomainEventsAsync(this);

            return await base.SaveChangesAsync(cancellationToken);
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_currentTransaction != null) return null;

            _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

            return _currentTransaction;
        }

        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null) 
                throw new ArgumentNullException(nameof(transaction));
            if (transaction != _currentTransaction) 
                throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

            try
            {
                await SaveChangesAsync();
                transaction.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
    }
}