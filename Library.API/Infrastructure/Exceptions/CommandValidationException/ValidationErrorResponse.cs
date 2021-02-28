using System.Collections.Generic;

namespace Library.API.Infrastructure.Exceptions.CommandValidationException
{
    public class ValidationErrorResponse
    {
        public ValidationErrorResponse()
        {
            Errors = new List<ValidationError>();
        }

        public IList<ValidationError> Errors { get; set; }
    }
}