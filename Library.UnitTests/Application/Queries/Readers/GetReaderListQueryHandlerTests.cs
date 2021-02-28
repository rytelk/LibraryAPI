using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Library.Application.DTOs;
using Library.Application.Mappers;
using Library.Application.Queries.Readers.GetReaderListQuery;
using Library.Domain.AggregatesModel.UserAggregate;
using Library.Infrastructure;
using Library.Infrastructure.Services;
using Xunit;

namespace Library.UnitTests.Application.Queries.Readers
{
    public class GetReaderListQueryHandlerTests
    {
        private readonly ILibraryContext _libraryContext;
        private readonly IMapper<User, UserDTO> _mapper;
        private readonly InMemoryLibraryContextFactory _libraryContextFactory;

        private GetReaderListQueryHandler _sut => new GetReaderListQueryHandler(_libraryContext, _mapper);

        public GetReaderListQueryHandlerTests()
        {
            _mapper = new UserMapper();
            _libraryContextFactory = new InMemoryLibraryContextFactory();
            _libraryContext = _libraryContextFactory.Create();
        }

        [Fact]
        public async Task Handle_ValidQuery_ReturnsOnlyReaders()
        {
            await SeedTestData(_libraryContext);

            var readers = await _sut.Handle(new GetReaderListQuery(), CancellationToken.None);

            readers.Count.Should().Be(2);
;        }

        private async Task SeedTestData(ILibraryContext context)
        {
            context.Users.Add(new User("FirstName", "LastName", new Email("email@gmail.com"), UserRolesConsts.Reader));
            context.Users.Add(new User("FirstName", "LastName", new Email("email@gmail.com"), UserRolesConsts.Reader));
            context.Users.Add(new User("FirstName", "LastName", new Email("email@gmail.com"), UserRolesConsts.Librarian));
            
            await context.SaveChangesAsync();
        }
    }
}