﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using ToDo.Microservices.Middleware.Querers;

namespace ToDo.Microservices.Middleware.Identities
{
    public static class IdentityStartupExtensions
    {
        public static void AddIdentity(this IServiceCollection services)
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
                client.DefaultRequestHeaders.Add(header.Key, header.Value.AsEnumerable());
            }

            return client;
        }

        public static void UseIdentity<TIdentityMiddleware>(this IApplicationBuilder app)
            where TIdentityMiddleware : IdentityMiddleware
        {
            app.UseMiddleware<TIdentityMiddleware>();
        }
    }
}
