using Nagornev.Querer.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ToDo.Domain.Results;

namespace ToDo.Microservices.Middleware.Identities
{
    public class IdentityResponseHandler : QuererHttpResponseMessageHandler<Result<Guid?>>
    {
        private class Interface2ClassConverter<TInterface, TClass> : JsonConverter
        {
            public override bool CanConvert(Type typeToConvert)
            {
                return typeToConvert == typeof(TInterface);
            }

            public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
            {
                return serializer.Deserialize<TClass>(reader);
            }

            public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
            {
                serializer.Serialize(writer, value);
            }
        }

        private JsonSerializer _serializer;

        public IdentityResponseHandler()
        {
            _serializer = new JsonSerializer();
            _serializer.Converters.Add(new Interface2ClassConverter<IError, Error>());
        }

        protected override void SetContent(ContentHandler handler)
        {
            handler.SetContent(response => response.GetContent((JToken json) => json.ToObject<Result<Guid?>>(_serializer)!));
        }

        protected override IEnumerable<Scheme.Set> SetScheme(Scheme scheme)
        {
            return scheme.Configure(scheme.Content);
        }
    }
}
