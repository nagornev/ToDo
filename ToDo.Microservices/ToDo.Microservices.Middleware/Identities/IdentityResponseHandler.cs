using Nagornev.Querer.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ToDo.Domain.Results;
using ToDo.Extensions.Converters;

namespace ToDo.Microservices.Middleware.Identities
{
    public class IdentityResponseHandler : QuererHttpResponseMessageHandler<Result<Guid?>>
    {
        private const string _errorMessage = "The identity service is unavalibable.";

        private JsonSerializer _serializer;

        public IdentityResponseHandler()
        {
            _serializer = new JsonSerializer();
            _serializer.Converters.Add(new Interface2ClassConverter<IError, Error>());
        }

        protected override void Configure(InvokerOptionsBuilder options)
        {
            options.SetFailure(options =>
                                options.AddFailure<Exception>((response, exception) => Result<Guid?>.Failure(error => error.InternalServer(_errorMessage))))
                   .SetLogger(options =>
                                options.AddAspLogger());
        }

        protected override void SetContent(ContentHandler handler)
        {
            handler.Set(response => response.GetContent((string text) => Result<Guid?>.Deserialize(text)!));
        }
    }
}
