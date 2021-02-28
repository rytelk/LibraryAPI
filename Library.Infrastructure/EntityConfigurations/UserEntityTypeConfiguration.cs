using Library.Domain.AggregatesModel.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.EntityConfigurations
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> userEntityTypeBuilder)
        {
            userEntityTypeBuilder.ToTable("Users", LibraryContext.DefaultSchema);
            userEntityTypeBuilder.HasKey(x => x.Id);
            userEntityTypeBuilder.Ignore(x => x.DomainEvents);

            userEntityTypeBuilder.Property(x => x.FirstName)
                .IsRequired();

            userEntityTypeBuilder.Property(x => x.LastName)
                .IsRequired();

            userEntityTypeBuilder.OwnsOne(x => x.Email, a =>
            {
                a.Property(p => p.EmailAddress)
                    .IsRequired();
            });

            userEntityTypeBuilder.Navigation(x => x.Email)
                .IsRequired();

            userEntityTypeBuilder.Property(x => x.PasswordHash)
                .IsRequired();

            userEntityTypeBuilder.Property(x => x.PasswordSalt)
                .IsRequired();

            userEntityTypeBuilder.Property(x => x.Role)
                .IsRequired();

        }
    }
}