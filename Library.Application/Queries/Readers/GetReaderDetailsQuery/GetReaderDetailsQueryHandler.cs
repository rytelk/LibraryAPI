using System.Threading;
using System.Threading.Tasks;
using Library.Application.DTOs;
using Library.Domain.AggregatesModel.UserAggregate;
using Library.Domain.Exceptions;
using Library.Infrastructure;
using Library.Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Library.Application.Queries.Readers.GetReaderDetailsQuery
{
    public class GetReaderDetailsQueryHandler : IRequestHandler<GetReaderDetailsQuery, UserDTO>
    {
        private readonly ILibraryContext _context;
        private readonly IMapper<User, UserDTO> _mapper;

        public GetReaderDetailsQueryHandler(ILibraryContext context, IMapper<User, UserDTO> mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserDTO> Handle(GetReaderDetailsQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == request.UserId && x.Role == UserRolesConsts.Reader);

            if(user == null)
            {
                throw new UserNotFoundException($"Reader with id: {request.UserId} was not found.");
            }

            return _mapper.Map(user);
        }
    }
}