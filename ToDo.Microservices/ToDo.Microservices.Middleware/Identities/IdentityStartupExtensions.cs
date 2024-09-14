using Microsoft.AspNetCore.Builder;

namespace ToDo.Microservices.Middleware.Identities
{
    public static class IdentityStartupExtensions
    {
        public static void UseIdentity<TIdentityMiddleware>(this IApplicationBuilder app)
            where TIdentityMiddleware : IdentityMiddleware
        {
            app.UseMiddleware<TIdentityMiddleware>();
        }
    }
}
