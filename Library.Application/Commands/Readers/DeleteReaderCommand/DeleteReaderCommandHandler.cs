using System.Threading;
using System.Threading.Tasks;
using Library.Domain.AggregatesModel.UserAggregate;
using Library.Domain.Exceptions;
using MediatR;

namespace Library.Application.Commands.Readers.DeleteReaderCommand
{
    public class DeleteReaderCommandHandler : IRequestHandler<DeleteReaderCommand>
    {
        private readonly IUserRepository _userRepository;

        public DeleteReaderCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(DeleteReaderCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(command.UserId);
            if (user == null)
            {
                throw new UserNotFoundException(command.UserId);
            }
            if (user.Role == UserRolesConsts.Librarian)
            {
                throw new LibraryDomainException("Cannot delete user with role librarian.");
            }
            if (user.HasAnyBookInHands())
            {
                throw new LibraryDomainException("Cannot delete user with unfinished loans.");
            }

            await _userRepository.DeleteAsync(user.Id);
            await _userRepository.UnitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}