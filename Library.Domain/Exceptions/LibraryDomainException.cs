using System;

namespace Library.Domain.Exceptions
{
    public class LibraryDomainException : Exception
    {
        public LibraryDomainException()
        { }

        public LibraryDomainException(string message)
            : base(message)
        { }

        public LibraryDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}