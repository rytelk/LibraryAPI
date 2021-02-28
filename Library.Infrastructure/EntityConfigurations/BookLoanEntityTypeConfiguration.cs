using Library.Domain.AggregatesModel.BookAggregate;
using Library.Domain.AggregatesModel.BookLoanAggregate;
using Library.Domain.AggregatesModel.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.EntityConfigurations
{
    public class BookLoanEntityTypeConfiguration : IEntityTypeConfiguration<BookLoan>
    {
        public void Configure(EntityTypeBuilder<BookLoan> bookLoanEntityTypeBuilder)
        {
            bookLoanEntityTypeBuilder.ToTable("BookLoans", LibraryContext.DefaultSchema);
            bookLoanEntityTypeBuilder.HasKey(x => x.Id);
            bookLoanEntityTypeBuilder.Ignore(x => x.DomainEvents);

            bookLoanEntityTypeBuilder.HasOne(x => x.User)
                .WithMany(x => x.BookLoans)
                .HasForeignKey(x => x.UserId)
                .IsRequired();

            bookLoanEntityTypeBuilder.HasOne(x => x.Book)
                .WithMany(x => x.BookLoans)
                .HasForeignKey(x => x.BookId)
                .IsRequired();

            bookLoanEntityTypeBuilder.Property(x => x.CreatedDate);

            bookLoanEntityTypeBuilder.Property(x => x.ReturnedDate)
                .IsRequired(false);

            bookLoanEntityTypeBuilder.Property(x => x.BorrowedDate)
                .IsRequired(false);

        }
    }
}