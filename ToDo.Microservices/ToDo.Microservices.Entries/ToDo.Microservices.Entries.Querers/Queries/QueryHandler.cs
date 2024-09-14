using Nagornev.Querer.Http;

namespace ToDo.Microservices.Entries.Querers.Queries
{
    public abstract class QueryHandler<TContentType> : QuererHttpResponseMessageHandler<TContentType>
    {
        protected const string SuccessToken = "success";

        protected const string ContentToken = "content";

        protected const string ErrorToken = "error";
    }
}
