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
            return await _userRepository.Get(userId);
        }

        public async Task<Result> CreateUser(Guid userId)
        {
            User user = User.Constructor(userId);

            return await _userRepository.Create(user);
        }
    }
}
