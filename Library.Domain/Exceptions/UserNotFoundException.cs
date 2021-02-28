namespace Library.Domain.Exceptions
{
    public class UserNotFoundException : DomainObjectNotFound
    {
        public UserNotFoundException(int userId)
            : base($"User with id: {userId} was not found.")
        {

        }

        public UserNotFoundException(string message)
            : base(message)
        {

        }
    }
}