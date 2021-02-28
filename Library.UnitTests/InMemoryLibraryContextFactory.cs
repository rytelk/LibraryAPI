using System;
using Library.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Library.UnitTests
{
    public class InMemoryLibraryContextFactory
    {
        public LibraryContext Create()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase($"LibraryContext{Guid.NewGuid()}")
                .Options;

            return new LibraryContext(options);
        }
    }
}