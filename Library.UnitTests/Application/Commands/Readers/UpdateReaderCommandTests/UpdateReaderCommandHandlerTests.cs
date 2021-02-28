using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Library.Application.Commands.Books.UpdateBookCommand;
using Library.Application.Commands.Readers.UpdateReaderCommand;
using Library.Application.DTOs;
using Library.Domain.AggregatesModel.UserAggregate;
using Library.Domain.SeedWork;
using Library.Infrastructure.Services;
using Moq;
using Xunit;

namespace Library.UnitTests.Application.Commands.Readers.UpdateReaderCommandTests
{
    public class UpdateReaderCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPasswordHashService> _passwordHashServiceMock;
        private readonly Mock<IMapper<User, UserDTO>> _mapperMock;

        private UpdateReaderCommandHandler _sut => new UpdateReaderCommandHandler(_userRepositoryMock.Object,
            _passwordHashServiceMock.Object, _mapperMock.Object);

        public UpdateReaderCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordHashServiceMock = new Mock<IPasswordHashService>();
            _mapperMock = new Mock<IMapper<User, UserDTO>>();

            _userRepositoryMock.SetupGet(x => x.UnitOfWork)
                .Returns(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_ReaderUpdated()
        {
            // Arrange
            var userId = 1;
            var newName = "NewFirstName";
            
            var user = new User("FirstName", "LastName", new Email("email@gmail.com"), UserRolesConsts.Reader);
            _userRepositoryMock.Setup(x => x.GetAsync(userId))
                .ReturnsAsync(user);

            var command = new UpdateReaderCommand()
            {
                UserId = userId,
                FirstName = newName,
                LastName = "LastName",
                Email = "email@gmail.com",
                Password = "password"
            };

            // Act
            await _sut.Handle(command, CancellationToken.None);

            // Assert
            _userRepositoryMock.Verify(x => x.Update(user));
            user.FirstName.Should().Be(newName);
        }
    }
}