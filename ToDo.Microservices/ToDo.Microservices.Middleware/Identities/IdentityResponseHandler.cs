using Nagornev.Querer.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ToDo.Domain.Results;
using ToDo.Extensions.Converters;

namespace ToDo.Microservices.Middleware.Identities
{
    public class IdentityResponseHandler : QuererHttpResponseMessageHandler<Result<Guid?>>
    {
        private JsonSerializer _serializer;

        public IdentityResponseHandler()
        {
            _serializer = new JsonSerializer();
            _serializer.Converters.Add(new Interface2ClassConverter<IError, Error>());
        }

        protected override void Configure(InvokerOptionsBuilder options)
        {
            options.SetFailure((response, exception) => Result<Guid?>.Failure(Errors.IsInternalServer($"Internal server.")))
                   .AddLogger();
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
