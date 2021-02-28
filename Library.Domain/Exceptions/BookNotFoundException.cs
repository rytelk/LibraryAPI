namespace Library.Domain.Exceptions
{
    public class BookNotFoundException : DomainObjectNotFound
    {
        public BookNotFoundException(int bookId)
            : base($"Book with id: {bookId} was not found.")
        {
            
        }
    }
}