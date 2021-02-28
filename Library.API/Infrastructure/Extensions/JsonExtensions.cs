using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Library.API.Infrastructure.Extensions
{
    public static class JsonExtensions
    {
        public static string SerializeToJson<TObject>(this TObject @this, bool indent = false)
            where TObject : class
        {
            var jsonOptions = new JsonSerializerSettings()
            {
                Formatting = indent ? Formatting.Indented : Formatting.None,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            return JsonConvert.SerializeObject(@this, jsonOptions);
        }
    }
}