using MediatR;

namespace Library.Application.Commands.Readers.DeleteReaderCommand
{
    public class DeleteReaderCommand : IRequest
    {
        public int UserId { get; set; }
    }
}