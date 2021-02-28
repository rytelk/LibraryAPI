using Library.Application.DTOs;
using MediatR;

namespace Library.Application.Commands.BookLoans.BookLoanCommand
{
    public class BookLoanCommand : IRequest<BookLoanInfoDTO>
    {
        public int BookId { get; set; }
        public int UserId { get; set; }
    }
}