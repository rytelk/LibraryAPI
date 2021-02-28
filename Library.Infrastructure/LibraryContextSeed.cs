using System;
using System.Linq;
using System.Threading.Tasks;
using Library.Domain.AggregatesModel.BookAggregate;
using Library.Domain.AggregatesModel.UserAggregate;
using Microsoft.Extensions.DependencyInjection;
using Library.Infrastructure.Services;

namespace Library.Infrastructure
{
    public class LibraryContextSeed
    {
        public async Task SeedAsync(LibraryContext context, IServiceProvider serviceProvider)
        {
            if (!context.Users.Any())
            {
                var passwordHashService = serviceProvider.GetService<IPasswordHashService>();
                
                var (passwordHashLibrarian, passwordSaltLibrarian) = passwordHashService.CreatePasswordHash("password");
                context.Users.Add(new User("FirstNameLibrarian", "LastNameLibrarian", new Email("librarian@gmail.com"),
                    passwordHashLibrarian, passwordSaltLibrarian, UserRolesConsts.Librarian));

                var (passwordHashReader, passwordSaltReader) = passwordHashService.CreatePasswordHash("password");
                context.Users.Add(new User("FirstNameReader", "LastNameReader", new Email("reader@gmail.com"),
                    passwordHashReader, passwordSaltReader, UserRolesConsts.Reader));
            }

            if (!context.Books.Any())
            {
                context.Books.Add(new Book("To Kill a Mockingbird", new Author("Harper", "Lee"), 1960,
                    "Beautiful story about how humans treat each other."));

                context.Books.Add(new Book("The Great Gatsby", new Author("Scott", "Fitzgerald"), 1925,
                    "The greatest, most scathing dissection of the hollowness at the heart of the American dream."));

                context.Books.Add(new Book("One Hundred Years of Solitude", new Author("Gabriel", "Marquez"), 1813,
                    "Both funny and moving."));

                context.Books.Add(new Book("In Cold Blood", new Author("Truman", "Capote"), 1965,
                    "The true crime TV show."));

                context.Books.Add(new Book("Go Set a Watchman", new Author("Harper", "Lee"), 2015,
                    "Stunning novel that is worth reading."));
            }

            await context.SaveChangesAsync();
        }
    }
}