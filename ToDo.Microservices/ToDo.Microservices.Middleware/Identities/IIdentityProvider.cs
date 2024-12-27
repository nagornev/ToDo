using Microsoft.AspNetCore.Http;

namespace ToDo.Microservices.Middleware.Identities
{
    public interface IIdentityProvider
    {
        Task Identify(RequestDelegate next, HttpContext context);
    }
}
