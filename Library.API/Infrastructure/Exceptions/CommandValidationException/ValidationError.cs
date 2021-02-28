namespace Library.API.Infrastructure.Exceptions.CommandValidationException
{
    public class ValidationError
    {
        public string FieldName { get; set; }
        public string Message { get; set; }
        public object AttemptedValue { get; set; }
    }
}