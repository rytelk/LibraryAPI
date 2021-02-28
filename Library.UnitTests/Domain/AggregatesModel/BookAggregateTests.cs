using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Library.Domain.AggregatesModel.BookAggregate;
using Xunit;
using FluentAssertions;
using Library.Domain.AggregatesModel.BookLoanAggregate;
using Library.Domain.Events;
using Library.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Library.UnitTests.Domain.AggregatesModel
{
    public class BookAggregateTests
    {
        private readonly InMemoryLibraryContextFactory _libraryContextFactory;

        public BookAggregateTests()
        {
            _libraryContextFactory = new InMemoryLibraryContextFactory();
        }

        [Fact]
        public void Create_BookValidData_CreatedSuccessfully()
        {
            var book = GetValidBook();

            book.Should().NotBeNull();
            book.InStock.Should().BeTrue();
        }

        [Fact]
        public void Create_BookInvalidData_ExceptionThrown()
        {
            string title = null;
            Author author = null;
            var yearPublished = 2020;
            string description = null;

            Action createBookAction = () => new Book(title, author, yearPublished, description);

            createBookAction.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Borrow_Book_NotInStock()
        {
            var book = GetValidBook();
            
            book.Borrow();

            book.InStock.Should().BeFalse();
        }

        [Fact]
        public void Return_Book_InStock()
        {
            var book = GetValidBook();
            book.Borrow();

            book.Return();

            book.InStock.Should().BeTrue();
        }

        [Fact]
        public void Return_Book_AddBookReturnedEvent()
        {
            var book = GetValidBook();
            book.Borrow();

            book.Return();

            book.DomainEvents.Count.Should().Be(1);
            var bookReturnedEvent = book.DomainEvents.First();
            (bookReturnedEvent is BookReturnedEvent).Should().BeTrue();
        }

        [Fact]
        public void GetReturnDueDate_BorrowedBook_ReturnDueDate()
        {
            // Arrange
            var context = _libraryContextFactory.Create();
            var book = GetValidBook();
            context.Books.Add(book);
            context.SaveChanges();

            var bookLoan = new BookLoan(book.Id, 1);
            bookLoan.SetBookBorrowed();
            context.BookLoans.Add(bookLoan);

            // Act
            book = context.Books.First();

            // Assert
            book.GetReturnDueDate().Should().Be(bookLoan.ReturnDueDate);
        }

        [Fact]
        public void GetReturnDueDate_BookInStock_ReturnDueDate()
        {
            var book = GetValidBook();

            book.GetReturnDueDate().Should().BeNull();
        }

        [Fact]
        public void GetFullName_Author_ConcatenatedFirstNameLastName()
        {
            var firstName = "William";
            var lastName = "Browns";
            var author = new Author(firstName, lastName);

            author.GetFullName().Should().Be($"{firstName} {lastName}");
        }

        [Theory]
        [InlineData("Jon", "Snow", "Jon", "Snow", true)]
        [InlineData("Jon", "Snow", "Jon", "Jones", false)]
        [InlineData("Will", "Cole", "Owen", "Shaw", false)]
        public void AuthorsEqualOperator_AuthorsData_Expected(string firstNameA, string lastNameA, string firstNameB, string lastNameB, bool expected)
        {
            var authorA = new Author(firstNameA, lastNameA);
            var authorB = new Author(firstNameB, lastNameB);

            var compare = authorA == authorB;

            compare.Should().Be(expected);
        }
        
        private static Book GetValidBook()
        {
            var title = "Title";
            var author = new Author("AuthorFirstName", "AuthorLastName");
            var yearPublished = 2020;
            var description = "Description";

            var book = new Book(title, author, yearPublished, description);
            return book;
        }
    }
}