using System.Threading;
using System.Threading.Tasks;
using Library.Application.Commands.Accounts.LoginCommand;
using Library.Application.Services;
using Moq;
using Xunit;

namespace Library.UnitTests.Application.Commands.Accounts.LoginCommandTests
{
    public class LoginCommandHandlerTests
    {
        private readonly Mock<IUserService> _userServiceMock;

        public LoginCommandHandlerTests()
        {
            _userServiceMock = new Mock<IUserService>();
        }

        private LoginCommandHandler _sut => new LoginCommandHandler(_userServiceMock.Object);

        [Fact]
        public async Task Handle_ValidCommand_UserLoggedIn()
        {
            var command = new LoginCommand()
            {
                Email = "email@gmail.com",
                Password = "password",
            };

            await _sut.Handle(command, CancellationToken.None);

            _userServiceMock.Verify(x => x.Login(command.Email, command.Password), Times.Once);
        }
    }
}