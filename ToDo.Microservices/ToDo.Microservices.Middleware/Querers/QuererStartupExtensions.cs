using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ToDo.Microservices.Middleware.Querers
{
    public static class QuererStartupExtensions
    {
        public static void AddQuererHttpClientFactory(this IServiceCollection services, Func<HttpContext, HttpClient> options)
        {
            services.AddSingleton(serviceProvider => new QuererHttpClientFactoryOptions(options));
            services.AddScoped<IQuererHttpClientFactory, QuererHttpClientFactory>();
        }
    }
}
