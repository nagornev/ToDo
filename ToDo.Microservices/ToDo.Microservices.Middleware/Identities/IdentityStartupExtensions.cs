using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ToDo.Extensions;

namespace ToDo.Microservices.Middleware.Identities
{
    public static class IdentityStartupExtensions
    {
        public static void AddIdentity(this IServiceCollection services)
        {
            services.AddRedirectableQuererHttpClientFactory();
        }

        public static void UseIdentity<TIdentityMiddleware>(this IApplicationBuilder app)
            where TIdentityMiddleware : IdentityMiddleware
        {
            app.UseMiddleware<TIdentityMiddleware>();
        }
    }
}
