using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Domain.Results;
using ToDo.Microservices.Identity.Database.Contexts;
using ToDo.Microservices.Identity.Domain.Models;
using ToDo.Microservices.Identity.Infrastructure.Providers;
using ToDo.Microservices.Identity.Infrastructure.Repositories;
using ToDo.Microservices.Identity.Infrastructure.Services;
using ToDo.Microservices.Identity.Tests.Mock;
using ToDo.Microservices.Identity.UseCases.Providers;
using ToDo.Microservices.Identity.UseCases.Publishers;
using ToDo.Microservices.Identity.UseCases.Repositories;
using ToDo.Microservices.Identity.UseCases.Services;

namespace ToDo.Microservices.Identity.Tests.Units
{
    public class UserServiceTests
    {
        public string _hashKey = "testtesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttest";

        public string _tokenKey = "testtesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttest";

        public int _tokenLifetime = 10000;

        public IHashProvider _hashProvider;

        public ITokenProvider _tokenProvider;

        public UserServiceTests()
        {
            _hashProvider = new PasswordHashProvider(Options.Create(new PasswordHashProviderOptions()
            {
                Key = _hashKey
            }));

            _tokenProvider = new JwtTokenProvider(Options.Create(new JwtTokenProviderOptions()
            {
                Key = _tokenKey,
                Lifetime = _tokenLifetime
            }));
        }

        private IdentityContext GetIdentityContextMock(Func<IReadOnlyCollection<User>> startData = default)
        {
            return new IdentityContextMock(Guid.NewGuid().ToString(), startData);
        }

        private IUserPublisher GetAvailableUserPublisherMock(Action<Mock<IUserPublisher>> configure = default)
        {
            return new UserPublisherMock(mock =>
            {
                mock.Setup(service => service.New(It.IsAny<User>()))
                    .Returns(async () => Result.Successful());

                configure?.Invoke(mock);
            });
        }

        private IUserPublisher GetUnavailableUserPublisherMock(Action<Mock<IUserPublisher>> configure = default)
        {
            return new UserPublisherMock(mock =>
            {
                mock.Setup(service => service.New(It.IsAny<User>()))
                    .Returns(async () => Result.Failure(error => error.InternalServer("The user publisher service is unavailable.")));

                configure?.Invoke(mock);
            });
        }

        #region GetUser

        [Fact]
        public async void UserService_GetUser_ById_UserExist_ShouldReturnSuccessTrueAndUserDomain()
        {
            //Arrage

            User[] users =
            {
                User.NewUser("user@test.ru", "jN5Ei0V6£[5&T+E9y65.R", (password) => _hashProvider.Hash(password)).Content,
                User.NewUser("john@test.ru", "4ji1!Da£)!+s@73b8i\\8ht", (password) => _hashProvider.Hash(password)).Content,
                User.NewSuper("super@test.ru", ",DqQ=X8bSu1'Be7uUA£U0!", (password) => _hashProvider.Hash(password)).Content,
            };

            IUserRepository userRepository = new UserRepository(GetIdentityContextMock(() => users),
                                                                GetUnavailableUserPublisherMock());

            IUserService userService = new UserService(userRepository,
                                                       _hashProvider,
                                                       _tokenProvider);


            User user = users.First();

            //Act

            Result<User> userResult = await userService.GetUser(user.Id);

            //Assert
            Assert.True(userResult.Success);
            Assert.NotNull(userResult.Content);
            Assert.Equal(user.Email, userResult.Content.Email);
            Assert.Equal(user.Password, userResult.Content.Password);
        }

        [Fact]
        public async void UserService_GetUser_ByEmail_UserExist_ShouldReturnSuccessTrueAndUserDomain()
        {
            //Arrage

            User[] users =
            {
                User.NewUser("user@test.ru", "jN5Ei0V6£[5&T+E9y65.R", (password) => _hashProvider.Hash(password)).Content,
                User.NewUser("john@test.ru", "4ji1!Da£)!+s@73b8i\\8ht", (password) => _hashProvider.Hash(password)).Content,
                User.NewSuper("super@test.ru", ",DqQ=X8bSu1'Be7uUA£U0!", (password) => _hashProvider.Hash(password)).Content,
            };

            IUserRepository userRepository = new UserRepository(GetIdentityContextMock(() => users),
                                                                GetUnavailableUserPublisherMock());

            IUserService userService = new UserService(userRepository,
                                                       _hashProvider,
                                                       _tokenProvider);


            User user = users.First();

            //Act

            Result<User> userResult = await userService.GetUser(user.Email);

            //Assert
            Assert.True(userResult.Success);
            Assert.NotNull(userResult.Content);
            Assert.Equal(user.Email, userResult.Content.Email);
            Assert.Equal(user.Password, userResult.Content.Password);
        }

        [Fact]
        public async void UserService_GetUser_ById_UserNotExist_ShouldReturnSuccessFalse()
        {
            //Arrage

            User[] users =
            {
                User.NewUser("john@test.ru", "4ji1!Da£)!+s@73b8i\\8ht", (password) => _hashProvider.Hash(password)).Content,
                User.NewSuper("super@test.ru", ",DqQ=X8bSu1'Be7uUA£U0!", (password) => _hashProvider.Hash(password)).Content,
            };

            IUserRepository userRepository = new UserRepository(GetIdentityContextMock(() => users),
                                                                GetUnavailableUserPublisherMock());

            IUserService userService = new UserService(userRepository,
                                                       _hashProvider,
                                                       _tokenProvider);


            User user = User.NewUser("user@test.ru", "jN5Ei0V6£[5&T+E9y65.R", (password) => _hashProvider.Hash(password)).Content;

            //Act

            Result<User> userResult = await userService.GetUser(user.Id);

            //Assert
            Assert.False(userResult.Success);
            Assert.NotNull(userResult.Error);
            Assert.Equal(ErrorKeys.NullOrEmpty, userResult.Error.Key);
        }


        [Fact]
        public async void UserService_GetUser_ByEmail_UserNotExist_ShouldReturnSuccessFalse()
        {
            //Arrage

            User[] users =
            {
                User.NewUser("john@test.ru", "4ji1!Da£)!+s@73b8i\\8ht", (password) => _hashProvider.Hash(password)).Content,
                User.NewSuper("super@test.ru", ",DqQ=X8bSu1'Be7uUA£U0!", (password) => _hashProvider.Hash(password)).Content,
            };

            IUserRepository userRepository = new UserRepository(GetIdentityContextMock(() => users),
                                                                GetUnavailableUserPublisherMock());

            IUserService userService = new UserService(userRepository,
                                                       _hashProvider,
                                                       _tokenProvider);


            User user = User.NewUser("user@test.ru", "jN5Ei0V6£[5&T+E9y65.R", (password) => _hashProvider.Hash(password)).Content;

            //Act

            Result<User> userResult = await userService.GetUser(user.Email);

            //Assert
            Assert.False(userResult.Success);
            Assert.NotNull(userResult.Error);
            Assert.Equal(ErrorKeys.NullOrEmpty, userResult.Error.Key);
        }

        #endregion

        #region SignUp

        [Fact]
        public async void UserService_SignUp_UserNotExistAndPublisherIsAvailable_ShouldReturnSuccessTrue()
        {
            //Arrage

            User[] users =
            {
                User.NewUser("john@test.ru", "4ji1!Da£)!+s@73b8i\\8ht", (password) => _hashProvider.Hash(password)).Content,
                User.NewSuper("super@test.ru", ",DqQ=X8bSu1'Be7uUA£U0!", (password) => _hashProvider.Hash(password)).Content,
            };

            IUserRepository userRepository = new UserRepository(GetIdentityContextMock(() => users),
                                                                GetAvailableUserPublisherMock());

            IUserService userService = new UserService(userRepository,
                                                       _hashProvider,
                                                       _tokenProvider);

            (string Email, string Password) user = ("user@test.ru", "jN5Ei0V6£[5&T+E9y65.R");

            //Act

            Result signUpResult = await userService.SignUp(user.Email, user.Password);

            //Assert
            Assert.True(signUpResult.Success);
        }

        [Fact]
        public async void UserService_SignUp_UserNotExistAndPublisherIsUnavailable_ShouldReturnSuccessFalse()
        {
            //Arrage

            User[] users =
            {
                User.NewUser("john@test.ru", "4ji1!Da£)!+s@73b8i\\8ht", (password) => _hashProvider.Hash(password)).Content,
                User.NewSuper("super@test.ru", ",DqQ=X8bSu1'Be7uUA£U0!", (password) => _hashProvider.Hash(password)).Content,
            };

            IUserRepository userRepository = new UserRepository(GetIdentityContextMock(() => users),
                                                                GetUnavailableUserPublisherMock());

            IUserService userService = new UserService(userRepository,
                                                       _hashProvider,
                                                       _tokenProvider);

            (string Email, string Password) user = ("user@test.ru", "jN5Ei0V6£[5&T+E9y65.R");

            //Act

            Result signUpResult = await userService.SignUp(user.Email, user.Password);

            //Assert
            Assert.False(signUpResult.Success);
            Assert.NotNull(signUpResult.Error);
            Assert.Equal(ErrorKeys.InternalServer, signUpResult.Error.Key);
        }

        [Fact]
        public async void UserService_SignUp_UserExistAndPublisherIsAvailable_ShouldReturnSuccessFalse()
        {
            //Arrage

            User[] users =
            {
                User.NewUser("user@test.ru", "jN5Ei0V6£[5&T+E9y65.R", (password) => _hashProvider.Hash(password)).Content,
                User.NewUser("john@test.ru", "4ji1!Da£)!+s@73b8i\\8ht", (password) => _hashProvider.Hash(password)).Content,
                User.NewSuper("super@test.ru", ",DqQ=X8bSu1'Be7uUA£U0!", (password) => _hashProvider.Hash(password)).Content,
            };

            IUserRepository userRepository = new UserRepository(GetIdentityContextMock(() => users),
                                                                GetAvailableUserPublisherMock());

            IUserService userService = new UserService(userRepository,
                                                       _hashProvider,
                                                       _tokenProvider);

            (string Email, string Password) user = ("user@test.ru", "jN5Ei0V6£[5&T+E9y65.R");

            //Act

            Result signUpResult = await userService.SignUp(user.Email, user.Password);

            //Assert
            Assert.False(signUpResult.Success);
            Assert.NotNull(signUpResult.Error);
            Assert.Equal(ErrorKeys.InvalidArgument, signUpResult.Error.Key);
        }

        [Fact]
        public async void UserService_SignUp_UserExistAndPublisherIsUnavailable_ShouldReturnSuccessFalse()
        {
            //Arrage

            User[] users =
            {
                User.NewUser("user@test.ru", "jN5Ei0V6£[5&T+E9y65.R", (password) => _hashProvider.Hash(password)).Content,
                User.NewUser("john@test.ru", "4ji1!Da£)!+s@73b8i\\8ht", (password) => _hashProvider.Hash(password)).Content,
                User.NewSuper("super@test.ru", ",DqQ=X8bSu1'Be7uUA£U0!", (password) => _hashProvider.Hash(password)).Content,
            };

            IUserRepository userRepository = new UserRepository(GetIdentityContextMock(() => users),
                                                                GetAvailableUserPublisherMock());

            IUserService userService = new UserService(userRepository,
                                                       _hashProvider,
                                                       _tokenProvider);

            (string Email, string Password) user = ("user@test.ru", "jN5Ei0V6£[5&T+E9y65.R");

            //Act

            Result signUpResult = await userService.SignUp(user.Email, user.Password);

            //Assert
            Assert.False(signUpResult.Success);
            Assert.NotNull(signUpResult.Error);
            Assert.Equal(ErrorKeys.InvalidArgument, signUpResult.Error.Key);
        }

        [Fact]
        public async void UserService_SignUp_UserNotExistAndPublisherIsAvailableAndInvalidEmail_ShouldReturnSuccessTrue()
        {
            //Arrage

            User[] users =
            {
                User.NewUser("john@test.ru", "4ji1!Da£)!+s@73b8i\\8ht", (password) => _hashProvider.Hash(password)).Content,
                User.NewSuper("super@test.ru", ",DqQ=X8bSu1'Be7uUA£U0!", (password) => _hashProvider.Hash(password)).Content,
            };

            IUserRepository userRepository = new UserRepository(GetIdentityContextMock(() => users),
                                                                GetAvailableUserPublisherMock());

            IUserService userService = new UserService(userRepository,
                                                       _hashProvider,
                                                       _tokenProvider);

            (string Email, string Password) user = ("user", "jN5Ei0V6£[5&T+E9y65.R");

            //Act

            Result signUpResult = await userService.SignUp(user.Email, user.Password);

            //Assert
            Assert.False(signUpResult.Success);
            Assert.NotNull(signUpResult.Error);
            Assert.Equal(ErrorKeys.InvalidArgument, signUpResult.Error.Key);
        }

        [Fact]
        public async void UserService_SignUp_UserNotExistAndPublisherIsAvailableAndInvalidPassword_ShouldReturnSuccessTrue()
        {
            //Arrage

            User[] users =
            {
                User.NewUser("john@test.ru", "4ji1!Da£)!+s@73b8i\\8ht", (password) => _hashProvider.Hash(password)).Content,
                User.NewSuper("super@test.ru", ",DqQ=X8bSu1'Be7uUA£U0!", (password) => _hashProvider.Hash(password)).Content,
            };

            IUserRepository userRepository = new UserRepository(GetIdentityContextMock(() => users),
                                                                GetAvailableUserPublisherMock());

            IUserService userService = new UserService(userRepository,
                                                       _hashProvider,
                                                       _tokenProvider);

            (string Email, string Password) user = ("user@test.ru", "qwerty");

            //Act

            Result signUpResult = await userService.SignUp(user.Email, user.Password);

            //Assert
            Assert.False(signUpResult.Success);
            Assert.NotNull(signUpResult.Error);
            Assert.Equal(ErrorKeys.InvalidArgument, signUpResult.Error.Key);
        }

        #endregion

        #region SignIn

        [Fact]
        public async void UserService_SignIn_UserExist_ShouldReturnSuccessTrueAndAccessToken()
        {
            //Arrage

            User[] users =
            {
                User.NewUser("user@test.ru", "jN5Ei0V6£[5&T+E9y65.R", (password) => _hashProvider.Hash(password)).Content,
                User.NewUser("john@test.ru", "4ji1!Da£)!+s@73b8i\\8ht", (password) => _hashProvider.Hash(password)).Content,
                User.NewSuper("super@test.ru", ",DqQ=X8bSu1'Be7uUA£U0!", (password) => _hashProvider.Hash(password)).Content,
            };

            IUserRepository userRepository = new UserRepository(GetIdentityContextMock(() => users),
                                                                GetUnavailableUserPublisherMock());

            IUserService userService = new UserService(userRepository,
                                                       _hashProvider,
                                                       _tokenProvider);


            (string Email, string Password) user = ("user@test.ru", "jN5Ei0V6£[5&T+E9y65.R");

            //Act

            Result<string> signInResult = await userService.SignIn(user.Email, user.Password);

            //Assert
            Assert.True(signInResult.Success);
            Assert.NotNull(signInResult.Content);
            Assert.NotEmpty(signInResult.Content);
            Assert.True(_tokenProvider.Validate(signInResult.Content, out string subject));
        }

        [Fact]
        public async void UserService_SignIn_UserNotExist_ShouldReturnSuccessFalse()
        {
            //Arrage

            User[] users =
            {
                User.NewUser("john@test.ru", "4ji1!Da£)!+s@73b8i\\8ht", (password) => _hashProvider.Hash(password)).Content,
                User.NewSuper("super@test.ru", ",DqQ=X8bSu1'Be7uUA£U0!", (password) => _hashProvider.Hash(password)).Content,
            };

            IUserRepository userRepository = new UserRepository(GetIdentityContextMock(() => users),
                                                                GetUnavailableUserPublisherMock());

            IUserService userService = new UserService(userRepository,
                                                       _hashProvider,
                                                       _tokenProvider);


            (string Email, string Password) user = ("user@test.ru", "jN5Ei0V6£[5&T+E9y65.R");

            //Act

            Result<string> signInResult = await userService.SignIn(user.Email, user.Password);

            //Assert
            Assert.False(signInResult.Success);
            Assert.NotNull(signInResult.Error);
            Assert.Equal(ErrorKeys.InvalidSignIn, signInResult.Error.Key);
        }

        [Fact]
        public async void UserService_SignIn_UserExistAndInvalidPassword_ShouldReturnSuccessFalse()
        {
            //Arrage

            User[] users =
            {
                User.NewUser("user@test.ru", "jN5Ei0V6£[5&T+E9y65.R", (password) => _hashProvider.Hash(password)).Content,
                User.NewUser("john@test.ru", "4ji1!Da£)!+s@73b8i\\8ht", (password) => _hashProvider.Hash(password)).Content,
                User.NewSuper("super@test.ru", ",DqQ=X8bSu1'Be7uUA£U0!", (password) => _hashProvider.Hash(password)).Content,
            };

            IUserRepository userRepository = new UserRepository(GetIdentityContextMock(() => users),
                                                                GetUnavailableUserPublisherMock());

            IUserService userService = new UserService(userRepository,
                                                       _hashProvider,
                                                       _tokenProvider);


            (string Email, string Password) user = ("user@test.ru", "qwerty");

            //Act

            Result<string> signInResult = await userService.SignIn(user.Email, user.Password);

            //Assert
            Assert.False(signInResult.Success);
            Assert.NotNull(signInResult.Error);
            Assert.Equal(ErrorKeys.InvalidSignIn, signInResult.Error.Key);
        }

        #endregion

        #region Validate

        [Fact]
        public async void UserService_Validate_UserExistAndTokenAuthorizatedAndPermissionUser_ShouldReturnSuccessTrue()
        {
            //Arrage

            User[] users =
            {
                User.NewUser("user@test.ru", "jN5Ei0V6£[5&T+E9y65.R", (password) => _hashProvider.Hash(password)).Content,
            };

            IUserRepository userRepository = new UserRepository(GetIdentityContextMock(() => users),
                                                                GetUnavailableUserPublisherMock());

            IUserService userService = new UserService(userRepository,
                                                       _hashProvider,
                                                       _tokenProvider);


            User user = users.First();

            string token = _tokenProvider.Create(user);

            //Act

            Result<Guid?> validateResult = await userService.Validate(token, new Permission[] { Permission.User });

            //Assert
            Assert.True(validateResult.Success);
            Assert.NotNull(validateResult.Content);
            Assert.Equal(user.Id, validateResult.Content);
        }

        [Fact]
        public async void UserService_Validate_UserExistAndTokenAuthorizatedAndPermissionSuper_ShouldReturnSuccessFalse()
        {
            //Arrage

            User[] users =
            {
                User.NewUser("user@test.ru", "jN5Ei0V6£[5&T+E9y65.R", (password) => _hashProvider.Hash(password)).Content,
            };

            IUserRepository userRepository = new UserRepository(GetIdentityContextMock(() => users),
                                                                GetUnavailableUserPublisherMock());

            IUserService userService = new UserService(userRepository,
                                                       _hashProvider,
                                                       _tokenProvider);


            User user = users.First();

            string token = _tokenProvider.Create(user);

            //Act

            Result<Guid?> validateResult = await userService.Validate(token, new Permission[] { Permission.Super });

            //Assert
            Assert.False(validateResult.Success);
            Assert.NotNull(validateResult.Error);
            Assert.Equal(ErrorKeys.Forbidden, validateResult.Error.Key);
        }

        [Fact]
        public async void UserService_Validate_SuperUserExistAndTokenAuthorizatedAndPermissionUser_ShouldReturnSuccessTrue()
        {
            //Arrage

            User[] users =
            {
                User.NewSuper("super@test.ru", ",DqQ=X8bSu1'Be7uUA£U0!", (password) => _hashProvider.Hash(password)).Content,
            };

            IUserRepository userRepository = new UserRepository(GetIdentityContextMock(() => users),
                                                                GetUnavailableUserPublisherMock());

            IUserService userService = new UserService(userRepository,
                                                       _hashProvider,
                                                       _tokenProvider);


            User user = users.First();

            string token = _tokenProvider.Create(user);

            //Act

            Result<Guid?> validateResult = await userService.Validate(token, new Permission[] { Permission.User });

            //Assert
            Assert.True(validateResult.Success);
            Assert.NotNull(validateResult.Content);
            Assert.Equal(user.Id, validateResult.Content);
        }

        [Fact]
        public async void UserService_Validate_SuperUserAndTokenAuthorizatedAndPermissionSuper_ShouldReturnSuccessTrue()
        {
            //Arrage

            User[] users =
            {
                User.NewSuper("super@test.ru", ",DqQ=X8bSu1'Be7uUA£U0!", (password) => _hashProvider.Hash(password)).Content,
            };

            IUserRepository userRepository = new UserRepository(GetIdentityContextMock(() => users),
                                                                GetUnavailableUserPublisherMock());

            IUserService userService = new UserService(userRepository,
                                                       _hashProvider,
                                                       _tokenProvider);


            User user = users.First();

            string token = _tokenProvider.Create(user);

            //Act

            Result<Guid?> validateResult = await userService.Validate(token, new Permission[] { Permission.Super });

            //Assert
            Assert.True(validateResult.Success);
            Assert.NotNull(validateResult.Content);
            Assert.Equal(user.Id, validateResult.Content);
        }

        [Fact]
        public async void UserService_Validate_UserExistAndTokenUnauthorizatedAndPermissionUser_ShouldReturnSuccessFalse()
        {
            //Arrage

            User[] users =
            {
                User.NewUser("user@test.ru", "jN5Ei0V6£[5&T+E9y65.R", (password) => _hashProvider.Hash(password)).Content,
            };

            IUserRepository userRepository = new UserRepository(GetIdentityContextMock(() => users),
                                                                GetUnavailableUserPublisherMock());

            IUserService userService = new UserService(userRepository,
                                                       _hashProvider,
                                                       _tokenProvider);

            ITokenProvider tokenProvider = new JwtTokenProvider(Options.Create(new JwtTokenProviderOptions()
            {
                Key = nameof(UserService_Validate_UserExistAndTokenUnauthorizatedAndPermissionUser_ShouldReturnSuccessFalse),
                Lifetime = _tokenLifetime
            }));


            User user = users.First();

            string token = tokenProvider.Create(user);

            //Act

            Result<Guid?> validateResult = await userService.Validate(token, new Permission[] { Permission.User });

            //Assert
            Assert.False(validateResult.Success);
            Assert.NotNull(validateResult.Error);
            Assert.Equal(ErrorKeys.Unauthorizated, validateResult.Error.Key);
        }

        #endregion
    }
}
