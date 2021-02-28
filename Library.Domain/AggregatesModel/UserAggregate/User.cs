using System;
using System.Collections.Generic;
using System.Linq;
using Library.Domain.AggregatesModel.BookLoanAggregate;
using Library.Domain.SeedWork;

namespace Library.Domain.AggregatesModel.UserAggregate
{
    public class User : Entity, IAggregateRoot
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Email Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Role { get; set; }

        private readonly List<BookLoan> _bookLoans = new List<BookLoan>();
        public IReadOnlyCollection<BookLoan> BookLoans => _bookLoans;

        protected User() { }

        public User(string firstName, string lastName, Email email, string role)
        {
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Role = role ?? throw new ArgumentNullException(nameof(role));
        }

        public User(string firstName, string lastName, Email email, byte[] passwordHash, byte[] passwordSalt, string role)
        {
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
            PasswordSalt = passwordSalt ?? throw new ArgumentNullException(nameof(passwordSalt));
            Role = role ?? throw new ArgumentNullException(nameof(role));
        }

        public int GetBooksInHandsCount()
        {
            return _bookLoans.Count(x => x.IsBorrowed);
        }

        public bool HasAnyBookInHands()
        {
            return GetBooksInHandsCount() > 0;
        }
    }
}