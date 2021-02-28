using System.Threading;
using System.Threading.Tasks;
using Library.Application.Commands.Books.CreateBookCommand;
using Library.Application.Commands.Readers.CreateReaderCommand;
using Library.Application.DTOs;
using Library.Application.Models;
using Library.Application.Services;
using Library.Domain.AggregatesModel.UserAggregate;
using Library.Infrastructure.Services;
using Moq;
using Xunit;

namespace Library.UnitTests.Application.Commands.Readers.CreateReaderCommandTests
{
    public class CreateReaderCommandHandlerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IMapper<User, UserDTO>> _mapperMock;

        private CreateReaderCommandHandler _sut => new CreateReaderCommandHandler(_userServiceMock.Object, _mapperMock.Object);

        public CreateReaderCommandHandlerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _mapperMock = new Mock<IMapper<User, UserDTO>>();
        }

        [Fact]
        public async Task Handle_ValidCommand_ReaderCreated()
        {
            var command = new CreateReaderCommand()
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "email@gmail.com",
                Password = "password"
            };

            await _sut.Handle(command, CancellationToken.None);
            
            _userServiceMock.Verify(x => x.Register(It.IsAny<RegisterUserModel>()), Times.Once);
        }

    }
}