using ToDo.Domain.Results;
using ToDo.Microservices.Categories.UseCases.Services;
using ToDo.Microservices.Middleware.Identities;

namespace ToDo.Microservices.Categories.API.Middlewares
{
    public class CategoriesIdentityChecker : IIdentityChecker
    {
        private IUserService _userService;

        public CategoriesIdentityChecker(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Result> Check(Guid userId)
        {
            return await _userService.GetUser(userId);
        }
    }
}
