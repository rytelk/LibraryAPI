using System.Collections.Generic;
using Library.Domain.Exceptions;
using Library.Domain.SeedWork;

namespace Library.Domain.AggregatesModel.UserAggregate
{
    public class Email : ValueObject
    {
        public string EmailAddress { get; private set; }



        public Email(string emailAddress)
        {
            if(!IsValidEmail(emailAddress))
            {
                throw new LibraryDomainException($"Invalid email address {emailAddress}.");
            }

            EmailAddress = emailAddress;
        }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return EmailAddress;
        }

        bool IsValidEmail(string email)
        {
            try
            {
                return new System.Net.Mail.MailAddress(email).Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}