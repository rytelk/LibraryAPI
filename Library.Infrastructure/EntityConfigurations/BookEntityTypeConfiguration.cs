using Library.Domain.AggregatesModel.BookAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.EntityConfigurations
{
    public class BookEntityTypeConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> bookEntityTypeBuilder)
        {
            bookEntityTypeBuilder.ToTable("Books", LibraryContext.DefaultSchema);
            bookEntityTypeBuilder.HasKey(x => x.Id);
            bookEntityTypeBuilder.Ignore(x => x.DomainEvents);

            bookEntityTypeBuilder.Property(x => x.Title)
                .IsRequired();

            bookEntityTypeBuilder.Property(x => x.YearPublished);

            bookEntityTypeBuilder.Property(x => x.Description)
                .IsRequired();

            bookEntityTypeBuilder.Property(x => x.InStock);

            bookEntityTypeBuilder.OwnsOne(x => x.Author, a =>
            {
                a.Property(p => p.FirstName)
                    .IsRequired();
                a.Property(p => p.LastName)
                    .IsRequired();
            });

            bookEntityTypeBuilder.Navigation(x => x.Author)
                .IsRequired();
        }
    }
}