using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Library.API.Infrastructure.Extensions;
using Library.API.Models;
using Library.Application.Commands.BookLoans.BookLoanCommand;
using Library.Application.Commands.BookLoans.BookReturnCommand;
using Library.Application.Commands.Books.CreateBookCommand;
using Library.Application.Commands.Books.DeleteBookCommand;
using Library.Application.Commands.Books.UpdateBookCommand;
using Library.Application.DTOs;
using Library.Application.Queries.Books.GetBookDetailsQuery;
using Library.Application.Queries.Books.GetBookListQuery;
using Library.Domain.AggregatesModel.UserAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Library.API.Controllers
{
    [Route("api/books")]
    [Authorize]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BooksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        ///     Get book details
        /// </summary>
        /// <param name="bookId">Book id</param>
        /// <response code="200">Book details</response>
        [HttpGet("{bookId:int}")]
        [ProducesResponseType(typeof(BookDetailsDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<BookDetailsDTO>> GetBookDetails(int bookId)
        {
            var book = await _mediator.Send(new GetBookDetailsQuery()
            {
                BookId = bookId,
                ShowQueueDetails = User.IsInRole(UserRolesConsts.Librarian)
            });
            return Ok(book);
        }

        /// <summary>
        ///     Get book list
        /// </summary>
        /// <param name="queryParameters">Search criteria</param>
        /// <response code="200">Book list matching criteria</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<BookListItemDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<BookListItemDTO>>> GetBooks([FromQuery] GetBookListParams queryParameters)
        {
            var books = await _mediator.Send(new GetBookListQuery()
            {
                Title = queryParameters.Title,
                Author = queryParameters.Author,
                YearPublished = queryParameters.YearPublished,
                Description = queryParameters.Description,
                InStock = queryParameters.InStock,
                ShowReturnDueDate = User.IsInRole(UserRolesConsts.Librarian)
            });
            return Ok(books);
        }


        /// <summary>
        ///     Create a book
        /// </summary>
        /// <param name="command">Book data</param>
        /// <response code="200">Created book details</response>
        [HttpPost]
        [Authorize(Roles = UserRolesConsts.Librarian)]
        [ProducesResponseType(typeof(BookDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<BookDTO>> CreateBook(CreateBookCommand command)
        {
            var book = await _mediator.Send(command);
            return Ok(book);
        }

        /// <summary>
        ///     Update a book
        /// </summary>
        /// <param name="command">Book data</param>
        /// <response code="200">Updated book details</response>
        [HttpPut]
        [Authorize(Roles = UserRolesConsts.Librarian)]
        [ProducesResponseType(typeof(BookDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<BookDTO>> UpdateBook(UpdateBookCommand command)
        {
            var book = await _mediator.Send(command);
            return Ok(book);
        }

        /// <summary>
        ///     Delete a book
        /// </summary>
        /// <param name="bookId">Book id</param>
        /// <response code="204"></response>
        [HttpDelete("{bookId:int}")]
        [Authorize(Roles = UserRolesConsts.Librarian)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteBook(int bookId)
        {
            await _mediator.Send(new DeleteBookCommand()
            {
                BookId = bookId
            });
            return NoContent();
        }

        /// <summary>
        ///     Borrow a book
        /// </summary>
        /// <param name="bookId">Book id</param>
        /// <response code="200">Book loan info</response>
        [HttpPost("{bookId:int}/borrow")]
        [ProducesResponseType(typeof(BookLoanInfoDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<BookLoanInfoDTO>> BorrowBook(int bookId)
        {
            var bookLoanInfo = await _mediator.Send(new BookLoanCommand()
            {
                BookId = bookId,
                UserId = User.GetUserId()
            });
            return Ok(bookLoanInfo);
        }

        /// <summary>
        ///     Return a book
        /// </summary>
        /// <param name="bookId">Book id</param>
        /// <response code="200"></response>
        [HttpPut("{bookId:int}/return")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> ReturnBook(int bookId)
        {
            await _mediator.Send(new BookReturnCommand()
            {
                BookId = bookId,
                UserId = User.GetUserId()
            });
            return Ok();
        }
    }
}
