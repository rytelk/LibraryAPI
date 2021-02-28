using System;
using System.Collections.Generic;
using Library.API.Infrastructure.Extensions;

namespace Library.API.Infrastructure.Exceptions.CommandValidationException
{
    public class CommandValidationException : Exception
    {
        public CommandValidationException(IList<ValidationError> validationErrors) : base(
            $"Validation error occurred:{Environment.NewLine}{validationErrors.SerializeToJson(true)}")
        {
            ValidationErrors = validationErrors;
        }

        public IList<ValidationError> ValidationErrors { get; protected set; }
    }
}