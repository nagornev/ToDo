using Nagornev.Querer.Http;

namespace ToDo.Microservices.Middleware.Querers
{
    public interface IQuererHttpClientFactory
    {
        QuererHttpClient Create();
    }
}
