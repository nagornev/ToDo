using Microsoft.AspNetCore.Http;
using ToDo.Domain.Results;
using ToDo.Microservices.Middleware.Identities;

namespace ToDo.Microservices.Middleware.Users
{
    public class UserValidatorProvider : IUserValidatorProvider
    {
        private readonly IUserValidator _userValidator;

        public UserValidatorProvider(IUserValidator userValidator)
        {
            _userValidator = userValidator;
        }

        public async Task Validate(RequestDelegate next, HttpContext context)
        {
            if (!context.User.Claims.Any(x => x.Type == IdentityDefaults.Subject))
            {
                await next.Invoke(context);
                return;
            }

            Guid userId = context.User.GetId();

            Result validateResult = await _userValidator.Validate(userId);

            if (!validateResult.Success)
            {
                BadRequest(context.Response, validateResult);
                return;
            }
            
            await next.Invoke(context);
        }

        private async void BadRequest(HttpResponse response, Result output)
        {
            response.StatusCode = output.Error!.Code;
            await response.WriteAsync(output.ToString());
        }
    }
}
