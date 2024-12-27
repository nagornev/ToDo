using Microsoft.AspNetCore.Http;

namespace ToDo.Microservices.Middleware.Users
{
    public class UserMiddleware
    {
        private readonly RequestDelegate _next;

        public UserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context,
                                      IUserValidatorProvider provider)
        {
            await provider.Validate(_next, context);
        }
    }
}
