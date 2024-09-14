using Nagornev.Querer.Http;

namespace ToDo.Microservices.Entries.Querers.Queries
{
    public abstract class QueryCompiler : QuererHttpRequestMessageCompiler
    {
        public string Url { get; private set; }

        public QueryCompiler(string url)
        {
            Url = url;
        }
    }
}
