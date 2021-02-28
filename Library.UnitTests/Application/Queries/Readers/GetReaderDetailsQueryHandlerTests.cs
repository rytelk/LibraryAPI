using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Library.Application.DTOs;
using Library.Application.Mappers;
using Library.Application.Queries.Readers.GetReaderDetailsQuery;
using Library.Domain.AggregatesModel.UserAggregate;
using Library.Domain.Exceptions;
using Library.Infrastructure;
using Library.Infrastructure.Services;
using Xunit;

namespace Library.UnitTests.Application.Queries.Readers
{
    public class GetReaderDetailsQueryHandlerTests
    {
        private readonly IMapper<User, UserDTO> _mapper;
        private readonly InMemoryLibraryContextFactory _libraryContextFactory;
        private readonly ILibraryContext _libraryContext;

        private GetReaderDetailsQueryHandler _sut => new GetReaderDetailsQueryHandler(_libraryContext, _mapper);

        public GetReaderDetailsQueryHandlerTests()
        {
            _mapper = new UserMapper();
            _libraryContextFactory = new InMemoryLibraryContextFactory();
            _libraryContext = _libraryContextFactory.Create();
        }

        [Fact]
        public async Task Handle_UserExists_UserDetails()
        {
            // Arrange
            var user = new User("FirstName", "LastName", new Email("email@gmail.com"), UserRolesConsts.Reader);
            _libraryContext.Users.Add(user);
            await _libraryContext.SaveChangesAsync();

            var query = new GetReaderDetailsQuery()
            {
                UserId = user.Id
            };

            // Act
            var userDetailsDto = await _sut.Handle(query, CancellationToken.None);

            // Assert
            userDetailsDto.Should().NotBeNull();
            userDetailsDto.Id.Should().Be(user.Id);
        }

        [Fact]
        public async Task Handle_UserNotExists_ExceptionThrown()
        {
            // Arrange
            var query = new GetReaderDetailsQuery()
            {
                UserId = 1
            };

            // Act
            Func<Task> getUserDetails = async () => await _sut.Handle(query, CancellationToken.None);

            // Assert
            getUserDetails.Should().Throw<UserNotFoundException>();
        }
    }
}