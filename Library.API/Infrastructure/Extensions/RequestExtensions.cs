namespace Library.API.Infrastructure.Extensions
{
    public static class RequestExtensions
    {
        public static string GetTypeName<TRequest>(this TRequest request)
        {
            return request.GetType().Name;
        }
    }
}