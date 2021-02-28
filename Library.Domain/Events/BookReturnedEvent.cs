using MediatR;

namespace Library.Domain.Events
{
    public class BookReturnedEvent : INotification
    {
        public int BookId { get; private set; }

        public BookReturnedEvent(int bookId)
        {
            BookId = bookId;
        }
    }
}