using System.Threading;
using System.Threading.Tasks;
using Library.Application.DTOs;
using Library.Domain.Exceptions;
using Library.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Library.Application.Queries.Books.GetBookDetailsQuery
{
    public class GetBookDetailsQueryHandler : IRequestHandler<GetBookDetailsQuery, BookDetailsDTO>
    {
        private readonly ILibraryContext _context;
        private readonly IBookDetailsMapper _mapper;

        public GetBookDetailsQueryHandler(ILibraryContext context, IBookDetailsMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BookDetailsDTO> Handle(GetBookDetailsQuery request, CancellationToken cancellationToken)
        {
            var book = await _context.Books
                .Include(x => x.BookLoans)
                    .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == request.BookId);

            if(book == null)
            {
                throw new BookNotFoundException(request.BookId);
            }

            return _mapper.Map(book, request.ShowQueueDetails);
        }
    }
}