namespace Library.Domain.Exceptions
{
    public class InvalidUserCredentialsException : LibraryDomainException
    {
        public InvalidUserCredentialsException()
            : base("Incorrect email or password.")
        {

        }
    }
}