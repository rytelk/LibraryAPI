using System;
using System.Collections.Generic;
using Library.Domain.SeedWork;

namespace Library.Domain.AggregatesModel.BookAggregate
{
    public class Author : ValueObject
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        public Author(string firstName, string lastName)
        {
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
        }

        public string GetFullName()
        {
            return $"{FirstName} {LastName}";
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return FirstName;
            yield return LastName;
        }
    }
}