using System.Threading;
using System.Threading.Tasks;
using Library.Application.Commands.Accounts.RegisterCommand;
using Library.Application.DTOs;
using Library.Application.Models;
using Library.Application.Services;
using Library.Domain.AggregatesModel.UserAggregate;
using Library.Infrastructure.Services;
using Moq;
using Xunit;

namespace Library.UnitTests.Application.Commands.Accounts.RegisterCommandTests
{
    public class RegisterCommandHandlerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IMapper<User, UserDTO>> _mapperMock;

        public RegisterCommandHandlerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _mapperMock = new Mock<IMapper<User, UserDTO>>();
        }

        private RegisterCommandHandler _sut => new RegisterCommandHandler(_userServiceMock.Object, _mapperMock.Object);


        [Fact]
        public async Task Handle_ValidCommand_UserRegistered()
        {
            var command = new RegisterCommand()
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "email@gmail.com",
                Password = "password",
                Role = UserRole.Librarian
            };

            await _sut.Handle(command, CancellationToken.None);

            _userServiceMock.Verify(x => x.Register(It.IsAny<RegisterUserModel>()), Times.Once);
        }
    }
}