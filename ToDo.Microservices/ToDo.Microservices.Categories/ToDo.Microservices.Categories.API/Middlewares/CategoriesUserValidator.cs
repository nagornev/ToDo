using ToDo.Domain.Results;
using ToDo.Microservices.Categories.UseCases.Services;
using ToDo.Microservices.Middleware.Users;

namespace ToDo.Microservices.Categories.API.Middlewares
{
    public class CategoriesUserValidator : IUserValidator
    {
        private IUserService _userService;

        public CategoriesUserValidator(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Result> Validate(Guid userId)
        {
            return await _userService.GetUser(userId);
        }
    }
}
