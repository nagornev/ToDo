using ToDo.Domain.Results;
using ToDo.Microservices.Identity.Domain.Models;
using ToDo.Microservices.Identity.UseCases.Publishers;
using ToDo.Microservices.Identity.UseCases.Providers;
using ToDo.Microservices.Identity.UseCases.Repositories;
using ToDo.Microservices.Identity.UseCases.Services;

namespace ToDo.Microservices.Identity.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;

        private IUserPublisher _userPublisher;

        private IHashProvider _hashProvider;

        private ITokenProvider _tokenProvider;

        public UserService(IUserRepository userRepository,
                           IUserPublisher userPublisher,
                           IHashProvider hashProvider,
                           ITokenProvider tokenProvider)
        {
            _userRepository = userRepository;
            _userPublisher = userPublisher;
            _hashProvider = hashProvider;
            _tokenProvider = tokenProvider;
        }

        public async Task<Result<User>> GetUser(Guid userId)
        {
            User? user = await _userRepository.Get(userId);

            return user is not null ?
                    Result<User>.Successful(user) :
                    Result<User>.Failure(Errors.IsNull($"The user ({userId}) was not found."));
        }

        public async Task<Result<User>> GetUser(string email)
        {
            User? user = await _userRepository.Get(email);

            return user is not null ?
                    Result<User>.Successful(user) :
                    Result<User>.Failure(Errors.IsNull($"The user ({email}) was not found."));
        }

        public async Task<Result<User>> SignUp(string email, string password)
        {
            if (await _userRepository.Get(email) is not null)
                return Result<User>.Failure(Errors.IsInvalidArgument($"The user ({email}) has already been registrated."));

            User user = User.NewUser(email, _hashProvider.Hash(password));

            return await _userRepository.Create(user) ?
                     ((await _userPublisher.New(user)).Success ?
                        Result<User>.Successful(user) :
                        Result<User>.Failure(Errors.IsMessage("Registration error. Please try again later."))) :
                     Result<User>.Failure(Errors.IsMessage("Registration error. Please try again later."));
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


        public async Task<Result<string>> SignIn(string email, string password, Action<string> callback)
        {
            Result<string> signInResult = await SignIn(email, password);

            if (signInResult.Success)
                callback.Invoke(signInResult.Content);

            return signInResult;
        }

        public async Task<Result<Guid?>> Validate(string token, IEnumerable<Permission> permissions)
        {
            if (!_tokenProvider.Validate(token, out string subject))
                return Result<Guid?>.Failure(Errors.IsUnauthorizated("Unauthorizated."));

            Result<User> resultUser = await GetUser(Guid.Parse(subject));

            return resultUser.Success ?
                    (resultUser.Content.Access.IsContained(permissions) ?
                        Result<Guid?>.Successful(resultUser.Content.Id) :
                        Result<Guid?>.Failure(Errors.IsForbidden("Forbidden."))) :
                    Result<Guid?>.Failure(resultUser.Error);
        }

    }
}
