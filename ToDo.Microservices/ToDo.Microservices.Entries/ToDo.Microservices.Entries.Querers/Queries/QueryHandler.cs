using Nagornev.Querer.Http;
using Newtonsoft.Json;
using ToDo.Domain.Results;
using ToDo.Extensions.Converters;

namespace ToDo.Microservices.Entries.Querers.Queries
{
    public abstract class QueryHandler<TContentType> : QuererHttpResponseMessageHandler<TContentType>
    {
        protected const string SuccessToken = "success";

        protected const string ContentToken = "content";

        protected const string ErrorToken = "error";

        protected JsonSerializer Serializer { get; private set; }

        public QueryHandler()
        {
            Serializer = new JsonSerializer();
            Serializer.Converters.Add(new Interface2ClassConverter<IError, Error>());
        }

        protected override IEnumerable<Scheme.Set> SetScheme(Scheme scheme)
        {
            return scheme.Configure(scheme.Content);
        }
    }
}
