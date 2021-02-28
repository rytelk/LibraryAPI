using System;
using System.Collections.Generic;
using System.Linq;
using Library.Domain.AggregatesModel.BookLoanAggregate;
using Library.Domain.Events;
using Library.Domain.SeedWork;

namespace Library.Domain.AggregatesModel.BookAggregate
{
    public class Book : Entity, IAggregateRoot
    {
        public string Title { get; set; }
        public Author Author { get; set; }
        public int YearPublished { get; set; }
        public string Description { get; set; }
        public bool InStock { get; private set; }

        private readonly List<BookLoan> _bookLoans = new List<BookLoan>();
        public IReadOnlyCollection<BookLoan> BookLoans => _bookLoans;

        protected Book()
        {
            InStock = true;
        }

        public Book(string title, Author author, int yearPublished, string description)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Author = author ?? throw new ArgumentNullException(nameof(author));
            YearPublished = yearPublished;
            Description = description ?? throw new ArgumentNullException(nameof(description));
            InStock = true;
        }

        public void Borrow()
        {
            InStock = false;
        }

        public void Return()
        {
            InStock = true;
            AddDomainEvent(new BookReturnedEvent(Id));
        }

        public DateTime? GetReturnDueDate()
        {
            return _bookLoans.FirstOrDefault(x => x.IsBorrowed)?.ReturnDueDate;
        }
    }
}