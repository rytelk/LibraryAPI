using System;
using Library.Domain.AggregatesModel.BookAggregate;
using Library.Domain.AggregatesModel.UserAggregate;
using Library.Domain.Events;
using Library.Domain.SeedWork;

namespace Library.Domain.AggregatesModel.BookLoanAggregate
{
    public class BookLoan : Entity, IAggregateRoot
    {
        public int BookId { get; private set; }
        public Book Book { get; private set; }
        public int UserId { get; private set; }
        public User User { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public DateTime? BorrowedDate { get; set; }
        public DateTime? ReturnedDate { get; private set; }

        public const int LoanDaysLimit = 30;
        public DateTime? ReturnDueDate => BorrowedDate?.AddDays(LoanDaysLimit);

        public bool IsBorrowed => BorrowedDate.HasValue && !ReturnedDate.HasValue;
        public bool IsReturned => ReturnedDate.HasValue;
        public bool IsPending => !BorrowedDate.HasValue && !ReturnedDate.HasValue;


        public BookLoan(int bookId, int userId)
        {
            BookId = bookId;
            UserId = userId;
            CreatedDate = DateTime.Now;
        }

        public void SetBookBorrowed()
        {
            BorrowedDate = DateTime.UtcNow;
        }

        public void FinishLoan()
        {
            ReturnedDate = DateTime.UtcNow;
            AddDomainEvent(new BookLoanFinishedEvent(UserId));
        }
    }
}