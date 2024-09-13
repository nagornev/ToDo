using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ToDo.Microservices.Middleware.Identities
{
    public static class IdentityStartupExtensions
    {
        public static void AddIdentity<TValidationMiddlewareType>(this IServiceCollection services)
            where TValidationMiddlewareType : IdentityMiddleware
        {
            services.AddSingleton<IdentityMiddleware, TValidationMiddlewareType>();
        }

        public static void UseIdentity(this IApplicationBuilder app)
        {
            app.UseMiddleware<IdentityMiddleware>();
        }
    }
}
