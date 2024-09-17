using ToDo.Domain.Results;
using ToDo.Microservices.Entries.Domain.Models;
using ToDo.Microservices.Entries.UseCases.Repositories;
using ToDo.Microservices.Entries.UseCases.Services;

namespace ToDo.Microservices.Entries.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<User>> GetUser(Guid userId)
        {
            User user = await _userRepository.Get(userId);

            return user is not null ?
                    Result<User>.Successful(user) :
                    Result<User>.Failure(Errors.IsNull($"The user ({userId}) was not found."));
        }

        public async Task<Result> CreateUser(Guid userId)
        {
            User user = User.Constructor(userId);

            return await _userRepository.Create(user) ?
                    Result.Successful() :
                    Result.Failure(Errors.IsMessage($"An error occurred when creating the user. Please try again later"));
        }
    }
}
