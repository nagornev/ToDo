using Microsoft.AspNetCore.Http;
using ToDo.Domain.Results;

namespace ToDo.Microservices.Middleware.Users
{
    public interface IUserValidatorProvider
    {
        Task Validate(RequestDelegate next, HttpContext context);
    }
}
