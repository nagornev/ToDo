using Microsoft.AspNetCore.Http;
using Nagornev.Querer.Http;

namespace ToDo.Microservices.Middleware.Querers
{
    public class QuererHttpClientFactory : IQuererHttpClientFactory
    {
        private HttpContext _context;

        private QuererHttpClientFactoryOptions _options;

        public QuererHttpClientFactory(IHttpContextAccessor accessor, QuererHttpClientFactoryOptions options)
        {
            _context = accessor.HttpContext;
            _options = options;
        }

        public QuererHttpClient Create()
        {
            HttpClient httpClient = _options.Configure(_context);

            return new QuererHttpClient(httpClient);
        }
    }
}
