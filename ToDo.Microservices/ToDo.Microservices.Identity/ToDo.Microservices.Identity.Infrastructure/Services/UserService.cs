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
                return Result<User>.Failure(error => error.InvalidArgument($"The user with email {email} has already been registrated."));

            Result<User> userResult = User.NewUser(email, password, (password) => _hashProvider.Hash(password));

            return userResult.Success ?
                        await _userRepository.Create(userResult.Content) :
                        userResult;

        }

        public async Task<Result<string>> SignIn(string email, string password)
        {
            Result<User> userResult = await GetUser(email);

            return userResult.Success ?
                    (_hashProvider.Verify(password, userResult.Content.Password) ?
                        Result<string>.Successful(_tokenProvider.Create(userResult.Content)) :
                        Result<string>.Failure(error => error.InvalidSignIn())) :
                    Result<string>.Failure(error => error.InvalidSignIn());
        }

        public async Task<Result<Guid?>> Validate(string token, IEnumerable<Permission> permissions)
        {
            if (!_tokenProvider.Validate(token, out string subject))
                return Result<Guid?>.Failure(error => error.Unauthorizated());

            Result<User> userResult = await GetUser(Guid.Parse(subject));

            return userResult.Success ?
                    (userResult.Content.Access.IsContained(permissions) ?
                        Result<Guid?>.Successful(userResult.Content.Id) :
                        Result<Guid?>.Failure(error => error.Forbidden())) :
                    Result<Guid?>.Failure(userResult.Error);
        }

    }
}
