using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Library.Application.DTOs;
using Library.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Library.Application.Queries.Books.GetBookListQuery
{
    public class GetBookListQueryHandler : IRequestHandler<GetBookListQuery, List<BookListItemDTO>>
    {
        private readonly ILibraryContext _context;
        private readonly IBookListMapper _mapper;

        public GetBookListQueryHandler(ILibraryContext context, IBookListMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<BookListItemDTO>> Handle(GetBookListQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Books
                .Include(x => x.BookLoans)
                .AsQueryable();


            if(!string.IsNullOrEmpty(request.Title))
            {
                query = query.Where(x => x.Title.Contains(request.Title));
            }

            if (!string.IsNullOrEmpty(request.Author?.FirstName))
            {
                query = query.Where(x => x.Author.FirstName.Contains(request.Author.FirstName));
            }

            if (!string.IsNullOrEmpty(request.Author?.LastName))
            {
                query = query.Where(x => x.Author.LastName.Contains(request.Author.LastName));
            }

            if (request.YearPublished.HasValue)
            {
                query = query.Where(x => x.YearPublished == request.YearPublished);
            }

            if (!string.IsNullOrEmpty(request.Description))
            {
                query = query.Where(x => x.Description.Contains(request.Description));
            }

            if (request.InStock.HasValue)
            {
                query = query.Where(x => x.InStock == request.InStock);
            }

            var books = await query.ToListAsync();

            return _mapper.MapBookList(books, request.ShowReturnDueDate);
        }
    }
}