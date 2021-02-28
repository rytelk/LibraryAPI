using System.Threading;
using System.Threading.Tasks;
using Library.Application.Commands.Readers.DeleteReaderCommand;
using Library.Domain.AggregatesModel.UserAggregate;
using Library.Domain.SeedWork;
using Moq;
using Xunit;

namespace Library.UnitTests.Application.Commands.Readers.DeleteReaderCommandTests
{
    public class DeleteReaderCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        private DeleteReaderCommandHandler _sut => new DeleteReaderCommandHandler(_userRepositoryMock.Object);

        public DeleteReaderCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userRepositoryMock = new Mock<IUserRepository>();

            _userRepositoryMock.SetupGet(x => x.UnitOfWork)
                .Returns(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_ReaderDeleted()
        {
            var userId = 1;
            var user = new User("FirstName", "LastName", new Email("email@gmail.com"), UserRolesConsts.Reader);
            _userRepositoryMock.Setup(x => x.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(user);
            var command = new DeleteReaderCommand()
            {
                UserId = userId
            };

            await _sut.Handle(command, CancellationToken.None);

            _userRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Once);
        }
    }
}