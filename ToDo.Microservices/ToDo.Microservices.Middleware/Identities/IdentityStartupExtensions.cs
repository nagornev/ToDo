using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToDo.Extensions;

namespace ToDo.Microservices.Middleware.Identities
{
    public static class IdentityStartupExtensions
    {
        public static void AddIdentity<TIdentityAttributeProvider>(this IServiceCollection services) 
            where TIdentityAttributeProvider: class, IIdentityAttributeProvider
        {
            services.AddScoped<IIdentityProvider, IdentityProvider>();
            services.AddRedirectableQuererHttpClientFactory();
            services.AddScoped<IIdentityAttributeProvider, TIdentityAttributeProvider>();
        }

        public static void UseIdentity(this IApplicationBuilder app)
        {
            app.UseMiddleware<IdentityMiddleware>();
        }
    }
}
