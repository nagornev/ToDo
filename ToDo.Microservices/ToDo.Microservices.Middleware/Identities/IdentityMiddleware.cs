using Microsoft.AspNetCore.Http;

namespace ToDo.Microservices.Middleware.Identities
{
    public class IdentityMiddleware
    {
        private readonly RequestDelegate _next;

        public IdentityMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context,
                                      IIdentityProvider provider)
        {
            await provider.Identify(_next, context);
        }
    }
}
