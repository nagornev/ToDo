using Nagornev.Querer.Http;

namespace ToDo.Extensions
{
    public interface IQuererHttpClientFactory
    {
        QuererHttpClient Create();
    }
}
