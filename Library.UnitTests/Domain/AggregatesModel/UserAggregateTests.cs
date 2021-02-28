using System;
using FluentAssertions;
using Library.Domain.AggregatesModel.BookLoanAggregate;
using Library.Domain.AggregatesModel.UserAggregate;
using Library.Domain.Exceptions;
using Xunit;

namespace Library.UnitTests.Domain.AggregatesModel
{
    public class UserAggregateTests
    {
        private readonly InMemoryLibraryContextFactory _libraryContextFactory;

        public UserAggregateTests()
        {
            _libraryContextFactory = new InMemoryLibraryContextFactory();
        }

        [Fact]
        public void Create_UserValidData_CreatedSuccessfully()
        {
            var user = GetValidUser();

            user.Should().NotBeNull();
        }

        [Fact]
        public void Create_UserInValidData_CreatedSuccessfully()
        {
            Action createUserAction = () => new User(null, null, null, null);

            createUserAction.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void GetBooksInHandsCount_UserWithBorrowedBook_CorrectNumberOfBooks()
        {
            // Arrange
            var context = _libraryContextFactory.Create();
            var user = GetValidUser();
            context.Users.Add(user);
            context.SaveChanges();

            var bookLoan = new BookLoan(1, user.Id);
            bookLoan.SetBookBorrowed();
            context.BookLoans.Add(bookLoan);

            // Act
            var userBooksInHandCount = user.GetBooksInHandsCount();
            var userHasAnyBookInHands = user.HasAnyBookInHands();

            // Assert
            userHasAnyBookInHands.Should().BeTrue();
            userBooksInHandCount.Should().Be(1);
        }

        [Fact]
        public void Create_InvalidEmail_ExceptionThrown()
        {
            Action emailCreate = () => new Email("not_valid_email");

            emailCreate.Should().Throw<LibraryDomainException>();
        }

        private static User GetValidUser()
        {
            return new User("FirstName", "LastName", new Email("email@gmail.com"), new byte[1],
                new byte[1], "Reader");
        }
    }
}