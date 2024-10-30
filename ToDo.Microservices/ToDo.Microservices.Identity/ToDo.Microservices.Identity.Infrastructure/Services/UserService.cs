using ToDo.Domain.Results;
using ToDo.Microservices.Identity.Domain.Models;
using ToDo.Microservices.Identity.UseCases.Providers;
using ToDo.Microservices.Identity.UseCases.Repositories;
using ToDo.Microservices.Identity.UseCases.Services;

namespace ToDo.Microservices.Identity.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;

        private IHashProvider _hashProvider;

        private ITokenProvider _tokenProvider;

        public UserService(IUserRepository userRepository,
                           IHashProvider hashProvider,
                           ITokenProvider tokenProvider)
        {
            _userRepository = userRepository;
            _hashProvider = hashProvider;
            _tokenProvider = tokenProvider;
        }

        public async Task<Result<User>> GetUser(Guid userId)
        {
            return await _userRepository.Get(userId);
        }

        public async Task<Result<User>> GetUser(string email)
        {
            return await _userRepository.Get(email);
        }

        public async Task<Result> SignUp(string email, string password)
        {
            if ((await _userRepository.Get(email)).Success)
                return Result<User>.Failure(Errors.IsInvalidArgument($"The user ({email}) has already been registrated."));

            User user = User.NewUser(email, _hashProvider.Hash(password));

            return await _userRepository.Create(user);

        }

        public async Task<Result<string>> SignIn(string email, string password)
        {
            Result<User> userResult = await GetUser(email);

            return userResult.Success ?
                    (_hashProvider.Verify(password, userResult.Content.Password) ?
                        Result<string>.Successful(_tokenProvider.Create(userResult.Content)) :
                        Result<string>.Failure(Errors.IsMessage("Please check your password and email and try again."))) :
                    Result<string>.Failure(Errors.IsMessage("Please check your password and email and try again."));
        }

        public async Task<Result<Guid?>> Validate(string token, IEnumerable<Permission> permissions)
        {
            if (!_tokenProvider.Validate(token, out string subject))
                return Result<Guid?>.Failure(Errors.IsUnauthorizated("Unauthorizated."));

            Result<User> userResult = await GetUser(Guid.Parse(subject));

            return userResult.Success ?
                    (userResult.Content.Access.IsContained(permissions) ?
                        Result<Guid?>.Successful(userResult.Content.Id) :
                        Result<Guid?>.Failure(Errors.IsForbidden("Forbidden."))) :
                    Result<Guid?>.Failure(userResult.Error);
        }

    }
}
