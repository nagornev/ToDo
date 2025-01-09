using Microsoft.AspNetCore.Http;

namespace ToDo.Microservices.Middleware.Users
{
    public interface IUserValidatorProvider
    {
        Task Validate(RequestDelegate next, HttpContext context);
    }
}
