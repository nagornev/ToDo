using ToDo.Domain.Results;
using ToDo.Microservices.Entries.UseCases.Services;
using ToDo.Microservices.Middleware.Users;

namespace ToDo.Microservices.Entries.API.Middlewares
{
    public class EntriesUserValidator : IUserValidator
    {
        private IUserService _userService;

        public EntriesUserValidator(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Result> Validate(Guid userId)
        {
            return await _userService.GetUser(userId);
        }
    }
}
