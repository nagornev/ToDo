using Microsoft.Extensions.Options;
using Moq;
using ToDo.Domain.Results;
using ToDo.Microservices.Identity.Database.Contexts;
using ToDo.Microservices.Identity.Domain.Models;
using ToDo.Microservices.Identity.Infrastructure.Providers;
using ToDo.Microservices.Identity.Infrastructure.Repositories;
using ToDo.Microservices.Identity.Tests.Mock;
using ToDo.Microservices.Identity.UseCases.Providers;
using ToDo.Microservices.Identity.UseCases.Publishers;
using ToDo.Microservices.Identity.UseCases.Repositories;

namespace ToDo.Microservices.Identity.Tests.Units
{
    public class UserRepositoryTests
    {
        public string _passwordHashKey = "testtesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttest";

        public IHashProvider _passwordHashProvider;

        public UserRepositoryTests()
        {
            _passwordHashProvider = new PasswordHashProvider(Options.Create(new PasswordHashProviderOptions() { Key = _passwordHashKey }));
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

        #region Get

        [Fact]
        public async void UserRepository_GetById_UserExist_ShouldReturnSuccessTrueAndUserInformation()
        {
            //Arrage

            User[] users =
            {
                User.NewUser("user@test.ru", "jN5Ei0V6£[5&T+E9y65.R", (password) => _passwordHashProvider.Hash(password)).Content,
                User.NewUser("john@test.ru", "4ji1!Da£)!+s@73b8i\\8ht", (password) => _passwordHashProvider.Hash(password)).Content,
                User.NewSuper("super@test.ru", ",DqQ=X8bSu1'Be7uUA£U0!", (password) => _passwordHashProvider.Hash(password)).Content,
            };

            IUserRepository userRepository = new UserRepository(GetIdentityContextMock(() => users),
                                                                GetUnavailableUserPublisherMock());


            User user = users.First();

            //Act

            Result<User> userResult = await userRepository.Get(user.Id);

            //Assert
            Assert.True(userResult.Success);
            Assert.NotNull(userResult.Content);
            Assert.Equal(user.Email, userResult.Content.Email);
            Assert.Equal(user.Password, userResult.Content.Password);
        }

        [Fact]
        public async void UserRepository_GetByEmail_UserExist_ShouldReturnSuccessTrueAndUserInformation()
        {
            //Arrage

            User[] users =
            {
                User.NewUser("user@test.ru", "jN5Ei0V6£[5&T+E9y65.R", (password) => _passwordHashProvider.Hash(password)).Content,
                User.NewUser("john@test.ru", "4ji1!Da£)!+s@73b8i\\8ht", (password) => _passwordHashProvider.Hash(password)).Content,
                User.NewSuper("super@test.ru", ",DqQ=X8bSu1'Be7uUA£U0!", (password) => _passwordHashProvider.Hash(password)).Content,
            };

            IUserRepository userRepository = new UserRepository(GetIdentityContextMock(() => users),
                                                                GetUnavailableUserPublisherMock());


            User user = users.First();

            //Act

            Result<User> userResult = await userRepository.Get(user.Email);

            //Assert
            Assert.True(userResult.Success);
            Assert.NotNull(userResult.Content);
            Assert.Equal(user.Email, userResult.Content.Email);
            Assert.Equal(user.Password, userResult.Content.Password);
        }

        [Fact]
        public async void UserRepository_GetById_UserNotExist_ShouldReturnSuccessFalse()
        {
            //Arrage

            User[] users =
            {
                User.NewUser("user@test.ru", "jN5Ei0V6£[5&T+E9y65.R", (password) => _passwordHashProvider.Hash(password)).Content,
                User.NewUser("john@test.ru", "4ji1!Da£)!+s@73b8i\\8ht", (password) => _passwordHashProvider.Hash(password)).Content,
                User.NewSuper("super@test.ru", ",DqQ=X8bSu1'Be7uUA£U0!", (password) => _passwordHashProvider.Hash(password)).Content,
            };

            IUserRepository userRepository = new UserRepository(GetIdentityContextMock(),
                                                                GetUnavailableUserPublisherMock());


            User user = users.First();

            //Act

            Result<User> userResult = await userRepository.Get(user.Id);

            //Assert
            Assert.False(userResult.Success);
            Assert.NotNull(userResult.Error);
            Assert.Equal(ErrorKeys.NullOrEmpty, userResult.Error.Key);
        }

        [Fact]
        public async void UserRepository_GetByEmail_UserNotExist_ShouldReturnSuccessFalse()
        {
            //Arrage

            User[] users =
            {
                User.NewUser("user@test.ru", "jN5Ei0V6£[5&T+E9y65.R", (password) => _passwordHashProvider.Hash(password)).Content,
                User.NewUser("john@test.ru", "4ji1!Da£)!+s@73b8i\\8ht", (password) => _passwordHashProvider.Hash(password)).Content,
                User.NewSuper("super@test.ru", ",DqQ=X8bSu1'Be7uUA£U0!", (password) => _passwordHashProvider.Hash(password)).Content,
            };

            IUserRepository userRepository = new UserRepository(GetIdentityContextMock(),
                                                                GetUnavailableUserPublisherMock());


            User user = users.First();

            //Act

            Result<User> userResult = await userRepository.Get(user.Id);

            //Assert
            Assert.False(userResult.Success);
            Assert.NotNull(userResult.Error);
            Assert.Equal(ErrorKeys.NullOrEmpty, userResult.Error.Key);
        }

        #endregion

        #region Create

        [Fact]
        public async void UserRepository_Create_UsersExistAndPublisherIsAvailable_ShouldReturnSuccessTrue()
        {
            //Arrage

            User[] users =
            {
                User.NewUser("john@test.ru", "4ji1!Da£)!+s@73b8i\\8ht", (password) => _passwordHashProvider.Hash(password)).Content,
                User.NewSuper("super@test.ru", ",DqQ=X8bSu1'Be7uUA£U0!", (password) => _passwordHashProvider.Hash(password)).Content,
            };

            IUserRepository userRepository = new UserRepository(GetIdentityContextMock(() => users),
                                                                GetAvailableUserPublisherMock());

            User user = User.NewUser("user@test.ru", "jN5Ei0V6£[5&T+E9y65.R", (password) => _passwordHashProvider.Hash(password)).Content;

            //Act

            Result createResult = await userRepository.Create(user);

            //Assert
            Assert.True(createResult.Success);
        }

        [Fact]
        public async void UserRepository_Create_UsersNotExistAndPublisherIsAvailable_ShouldReturnSuccessTrue()
        {
            //Arrage

            IUserRepository userRepository = new UserRepository(GetIdentityContextMock(),
                                                                GetAvailableUserPublisherMock());

            User user = User.NewUser("user@test.ru", "jN5Ei0V6£[5&T+E9y65.R", (password) => _passwordHashProvider.Hash(password)).Content;

            //Act

            Result createResult = await userRepository.Create(user);

            //Assert
            Assert.True(createResult.Success);
        }

        [Fact]
        public async void UserRepository_Create_UsersExistAndPublisherIsUnavailable_ShouldReturnSuccessFalse()
        {
            //Arrage

            User[] users =
            {
                User.NewUser("john@test.ru", "4ji1!Da£)!+s@73b8i\\8ht", (password) => _passwordHashProvider.Hash(password)).Content,
                User.NewSuper("super@test.ru", ",DqQ=X8bSu1'Be7uUA£U0!", (password) => _passwordHashProvider.Hash(password)).Content,
            };

            IUserRepository userRepository = new UserRepository(GetIdentityContextMock(() => users),
                                                                GetUnavailableUserPublisherMock());

            User user = User.NewUser("user@test.ru", "jN5Ei0V6£[5&T+E9y65.R", (password) => _passwordHashProvider.Hash(password)).Content;

            //Act

            Result createResult = await userRepository.Create(user);

            //Assert
            Assert.False(createResult.Success);
        }

        [Fact]
        public async void UserRepository_Create_UsersNotExistAndPublisherIsUnavailable_ShouldReturnSuccessFalse()
        {
            //Arrage

            IUserRepository userRepository = new UserRepository(GetIdentityContextMock(),
                                                                GetUnavailableUserPublisherMock());

            User user = User.NewUser("user@test.ru", "jN5Ei0V6£[5&T+E9y65.R", (password) => _passwordHashProvider.Hash(password)).Content;

            //Act

            Result createResult = await userRepository.Create(user);

            //Assert
            Assert.False(createResult.Success);
        }

        #endregion
    }
}
