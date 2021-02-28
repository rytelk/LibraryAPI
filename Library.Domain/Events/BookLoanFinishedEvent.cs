using MediatR;

namespace Library.Domain.Events
{
    public class BookLoanFinishedEvent : INotification
    {
        public int UserId { get; private set; }

        public BookLoanFinishedEvent(int userId)
        {
            UserId = userId;
        }
    }
}