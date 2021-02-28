using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Library.Application.Commands.Accounts.RegisterCommand
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum UserRole
    {
        Reader,
        Librarian
    }
}