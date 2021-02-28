using System.Linq;
using FluentAssertions;
using Library.Domain.AggregatesModel.BookLoanAggregate;
using Library.Domain.Events;
using Xunit;

namespace Library.UnitTests.Domain.AggregatesModel
{
    public class BookLoanAggregateTests
    {

        [Fact]
        public void Create_BookLoan_CreatedPendingBookLoan()
        {
            var bookLoan = new BookLoan(1, 1);

            bookLoan.Should().NotBeNull();
            bookLoan.IsPending.Should().BeTrue();
            bookLoan.IsBorrowed.Should().BeFalse();
            bookLoan.IsReturned.Should().BeFalse();
        }

        [Fact]
        public void SetBookBorrowed_BookLoan_BookLoanInBorrowedState()
        {
            var bookLoan = new BookLoan(1, 1);

            bookLoan.SetBookBorrowed();

            bookLoan.IsPending.Should().BeFalse();
            bookLoan.IsBorrowed.Should().BeTrue();
            bookLoan.IsReturned.Should().BeFalse();
        }

        [Fact]
        public void FinishLoan_BookLoan_BookLoanInReturnedState()
        {
            var bookLoan = new BookLoan(1, 1);

            bookLoan.FinishLoan();

            bookLoan.IsPending.Should().BeFalse();
            bookLoan.IsBorrowed.Should().BeFalse();
            bookLoan.IsReturned.Should().BeTrue();
        }

        [Fact]
        public void FinishLoan_BookLoan_AddBookLoanFinishedEvent()
        {
            var bookLoan = new BookLoan(1, 1);

            bookLoan.FinishLoan();

            bookLoan.DomainEvents.Count.Should().Be(1);
            var bookReturnedEvent = bookLoan.DomainEvents.First();
            (bookReturnedEvent is BookLoanFinishedEvent).Should().BeTrue();
        }

        [Fact]
        public void ReturnDueDate_BorrowedBookLoan_CorrectReturnDueDate()
        {
            var bookLoan = new BookLoan(1, 1);

            bookLoan.SetBookBorrowed();

            bookLoan.ReturnDueDate.Should().Be(bookLoan.BorrowedDate.Value.AddDays(BookLoan.LoanDaysLimit));
        }

        [Fact]
        public void ReturnDueDate_NotBorrowedBookLoan_NullReturnDueDate()
        {
            var bookLoan = new BookLoan(1, 1);

            bookLoan.ReturnDueDate.Should().BeNull();
        }
    }
}