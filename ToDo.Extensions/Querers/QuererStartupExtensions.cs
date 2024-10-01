using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace ToDo.Extensions
{
    public static class QuererStartupExtensions
    {
        public static void AddRedirectableQuererHttpClientFactory(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddQuererHttpClientFactory(context =>
            {
                HttpMessageHandler handler = GetHttpMessageHandler(context);
                HttpClient client = GetHttpClient(handler, context);

                return client;
            });
        }

        private static HttpMessageHandler GetHttpMessageHandler(HttpContext context)
        {
            HttpClientHandler handler = new HttpClientHandler();

            handler.UseCookies = true;
            handler.CookieContainer = new CookieContainer();

            Uri url = new Uri($"http://{context.Request.Host.Value}");

            foreach (var cookie in context.Request.Cookies)
            {
                handler.CookieContainer.Add(url, new Cookie(cookie.Key, cookie.Value));
            }

            return handler;
        }

        private static HttpClient GetHttpClient(HttpMessageHandler handler, HttpContext context)
        {
            HttpClient client = new HttpClient(handler);

            foreach (var header in context.Request.Headers)
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value.AsEnumerable());
            }

            return client;
        }

        public static void AddQuererHttpClientFactory(this IServiceCollection services, Func<HttpContext, HttpClient> options)
        {
            services.AddSingleton(serviceProvider => new QuererHttpClientFactoryOptions(options));
            services.AddScoped<IQuererHttpClientFactory, QuererHttpClientFactory>();
        }
    }
}
