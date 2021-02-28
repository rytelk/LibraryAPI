using System.Collections.Generic;
using Library.Application.DTOs;
using MediatR;

namespace Library.Application.Queries.Readers.GetReaderListQuery
{
    public class GetReaderListQuery : IRequest<List<UserDTO>>
    {
    }
}