using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Library.Application.DTOs;
using Library.Domain.AggregatesModel.UserAggregate;
using Library.Infrastructure;
using Library.Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Library.Application.Queries.Readers.GetReaderListQuery
{
    public class GetReaderListQueryHandler : IRequestHandler<GetReaderListQuery, List<UserDTO>>
    {
        private readonly ILibraryContext _context;
        private readonly IMapper<User, UserDTO> _mapper;

        public GetReaderListQueryHandler(ILibraryContext context, IMapper<User, UserDTO> mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<UserDTO>> Handle(GetReaderListQuery request, CancellationToken cancellationToken)
        {
            var users = await _context.Users
                .Where(x => x.Role == UserRolesConsts.Reader)
                .ToListAsync();
            return users.Select(_mapper.Map).ToList();
        }
    }
}