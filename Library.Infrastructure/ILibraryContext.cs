using Library.Domain.AggregatesModel.BookAggregate;
using Library.Domain.AggregatesModel.BookLoanAggregate;
using Library.Domain.AggregatesModel.UserAggregate;
using Library.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure
{
    public interface ILibraryContext : IUnitOfWork
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<BookLoan> BookLoans { get; set; }
        public DbSet<User> Users { get; set; }

    }
}