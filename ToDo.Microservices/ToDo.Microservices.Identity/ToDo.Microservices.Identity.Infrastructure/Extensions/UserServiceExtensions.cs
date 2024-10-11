using ToDo.Domain.Results;
using ToDo.Microservices.Identity.UseCases.Services;

namespace ToDo.Microservices.Identity.Infrastructure.Extensions
{
    public static class UserServiceExtensions
    {
        public static async Task<Result<string>> SignIn(this IUserService userService, string email, string password, Action<string> callback)
        {
            Result<string> signInResult = await userService.SignIn(email, password);

            if (signInResult.Success)
                callback.Invoke(signInResult.Content);

            return signInResult;
        }
    }
}
