using ToDo.Domain.Results;
using ToDo.Microservices.Entries.UseCases.Services;
using ToDo.Microservices.Middleware.Identities;

namespace ToDo.Microservices.Entries.API.Middlewares
{
    public class EntriesIdentityChecker : IIdentityChecker
    {
        private IUserService _userService;

        public EntriesIdentityChecker(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Result> Check(Guid userId)
        {
            return await _userService.GetUser(userId);
        }
    }
}
