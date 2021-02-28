namespace Library.Domain.Exceptions
{
    public class DomainObjectNotFound : LibraryDomainException
    {
        public DomainObjectNotFound(string message)
            : base(message)
        { }
    }
}