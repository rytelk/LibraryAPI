using Library.Application.DTOs;
using MediatR;

namespace Library.Application.Queries.Readers.GetReaderDetailsQuery
{
    public class GetReaderDetailsQuery : IRequest<UserDTO>
    {
        public int UserId { get; set; }
    }
}