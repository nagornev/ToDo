using ToDo.Microservices.Categories.Infrastructure.Repositories;
using ToDo.Microservices.Categories.Tests.Mock;
using ToDo.Microservices.Categories.UseCases.Repositories;
using Moq;
using ToDo.Domain.Results;
using ToDo.Microservices.Categories.Domain.Models;
using ToDo.Microservices.Categories.UseCases.Publishers;
using Microsoft.EntityFrameworkCore;

namespace ToDo.Microservices.Categories.Tests.Units
{
    public class CategoryRepositoryTests
    {
        private CategoryContextMock GetCategoryContextMock( Func<IReadOnlyDictionary<User, IEnumerable<Category>>> startContext = null)
        {
            return new CategoryContextMock($"{Guid.NewGuid()}", startContext);
        }

        private CategoryPublisherMock GetCategoryPublisherMock(Action<Mock<ICategoryPubliser>> configure = null)
        {
            return new CategoryPublisherMock((mock) =>
            {
                mock.Setup(_ => _.Delete(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(async () => Result.Successful());

                configure?.Invoke(mock);
            });
        }

        #region Get

        [Fact]
        public async void CategoryRepository_Get_UserExistAndCategoryCollectionIsNotEmpty_ShouldReturnSuccessTrueAndCategoryCollection()
        {
            //Arrage

            User user = User.Constructor(Guid.NewGuid());

            Category[] categories =
            {
                Category.Constructor(Guid.NewGuid(), user.Id.ToString()),
                Category.Constructor(Guid.NewGuid(), user.Id.ToString()),
                Category.Constructor(Guid.NewGuid(), user.Id.ToString()),
            };

            ICategoryRepository categoryRepository = new CategoryRepository(GetCategoryContextMock(() => new Dictionary<User, IEnumerable<Category>>()
                                                                                                           {
                                                                                                                {user, categories}
                                                                                                           }),
                                                                            GetCategoryPublisherMock());

            //Act

            Result<IEnumerable<Category>> categoriesResult = await categoryRepository.Get(user.Id);

            //Assert

            Assert.True(categoriesResult.Success);
            Assert.NotEmpty(categoriesResult.Content);
            Assert.All(categoriesResult.Content, (category) => Assert.Contains(user.Id.ToString(), category.Name));
        }

        [Fact]
        public async void CategoryRepository_Get_UserExistAndCategoryCollectionIsEmpty_ShouldReturnSuccessTrueAndEmptyCategoryCollection()
        {
            //Arrage

            User user = User.Constructor(Guid.NewGuid());

            ICategoryRepository categoryRepository = new CategoryRepository(GetCategoryContextMock(() => new Dictionary<User, IEnumerable<Category>>()
                                                                                                           {
                                                                                                                {user, Enumerable.Empty<Category>()}
                                                                                                           }),
                                                                            GetCategoryPublisherMock());

            //Act

            Result<IEnumerable<Category>> categoriesResult = await categoryRepository.Get(user.Id);

            //Assert

            Assert.True(categoriesResult.Success);
            Assert.Empty(categoriesResult.Content);
        }

        [Fact]
        public async void CategoryRepository_Get_UserNotExistAndCategoryCollectionNotExist_ShouldReturnSuccessFalse()
        {
            //Arrage

            ICategoryRepository categoryRepository = new CategoryRepository(GetCategoryContextMock(),
                                                                            GetCategoryPublisherMock());

            User user = User.Constructor(Guid.NewGuid());

            //Act

            Result<IEnumerable<Category>> categoriesResult = await categoryRepository.Get(user.Id);

            //Assert

            Assert.False(categoriesResult.Success);
        }

        [Fact]
        public async void CategoryRepository_Get_UserExistAndCategoryCollectionIsNotEmpty_ShouldReturnSuccessTrueAndCategory()
        {
            //Arrage

            User user = User.Constructor(Guid.NewGuid());

            Category[] categories =
            {
                Category.Constructor(Guid.NewGuid(), user.Id.ToString()),
                Category.Constructor(Guid.NewGuid(), user.Id.ToString()),
                Category.Constructor(Guid.NewGuid(), user.Id.ToString()),
            };

            ICategoryRepository categoryRepository = new CategoryRepository(GetCategoryContextMock(() => new Dictionary<User, IEnumerable<Category>>()
                                                                                                           {
                                                                                                                {user, categories}
                                                                                                           }),
                                                                            GetCategoryPublisherMock());

            Category receiveCategory = categories.First();

            //Act

            Result<Category> categoryResult = await categoryRepository.Get(user.Id, receiveCategory.Id);

            //Assert

            Assert.True(categoryResult.Success);
            Assert.Equal(receiveCategory.Id.ToString(), categoryResult.Content.Id.ToString());
        }

        [Fact]
        public async void CategoryRepository_Get_UserExistAndCategoryCollectionIsEmpty_ShouldReturnSuccessFalse()
        {
            //Arrage

            User user = User.Constructor(Guid.NewGuid());

            ICategoryRepository categoryRepository = new CategoryRepository(GetCategoryContextMock(() => new Dictionary<User, IEnumerable<Category>>()
                                                                                                           {
                                                                                                                {user, Enumerable.Empty<Category>()}
                                                                                                           }),
                                                                            GetCategoryPublisherMock());

            Category receiveCategory = Category.New(user.Id.ToString());

            //Act

            Result<Category> categoryResult = await categoryRepository.Get(user.Id, receiveCategory.Id);

            //Assert

            Assert.False(categoryResult.Success);
        }

        [Fact]
        public async void CategoryRepository_Get_UsersExistAndCategoryCollectionsIsNotEmpty_ShouldReturnSuccessFalse()
        {
            //Arrage

            User firstUser = User.Constructor(Guid.NewGuid());

            Category[] firstCategories =
            {
                Category.Constructor(Guid.NewGuid(), firstUser.Id.ToString()),
                Category.Constructor(Guid.NewGuid(), firstUser.Id.ToString()),
                Category.Constructor(Guid.NewGuid(), firstUser.Id.ToString()),
            };

            User secondUser = User.Constructor(Guid.NewGuid());

            Category[] secondCategories =
            {
                Category.Constructor(Guid.NewGuid(), secondUser.Id.ToString()),
                Category.Constructor(Guid.NewGuid(), secondUser.Id.ToString()),
                Category.Constructor(Guid.NewGuid(), secondUser.Id.ToString()),
            };

            ICategoryRepository categoryRepository = new CategoryRepository(GetCategoryContextMock(() => new Dictionary<User, IEnumerable<Category>>()
                                                                                                           {
                                                                                                                {firstUser, firstCategories},
                                                                                                                {secondUser, secondCategories},
                                                                                                           }),
                                                                            GetCategoryPublisherMock());

            Category receiveCategory = secondCategories.First();

            //Act

            Result<Category> categoryResult = await categoryRepository.Get(firstUser.Id, receiveCategory.Id);

            //Assert

            Assert.False(categoryResult.Success);
        }

        #endregion

        #region Create

        [Fact]
        public async void CategoryRepository_Create_UserExistAndCategoryCollectionIsEmpty_ShouldReturnSuccessTrue()
        {
            //Arrage

            User user = User.Constructor(Guid.NewGuid());

            ICategoryRepository categoryRepository = new CategoryRepository(GetCategoryContextMock(() => new Dictionary<User, IEnumerable<Category>>()
                                                                                                           {
                                                                                                                {user, Enumerable.Empty<Category>()}
                                                                                                           }),
                                                                            GetCategoryPublisherMock());

            Category newCategory = Category.New(user.Id.ToString());

            //Act

            Result createResult = await categoryRepository.Create(user.Id, newCategory);

            //Assert

            Assert.True(createResult.Success);
        }

        [Fact]
        public async void CategoryRepository_Create_UserNotExistAndCategoryCollectionIsEmpty_ShouldThrowDbUpdateException()
        {
            //Arrage

            ICategoryRepository categoryRepository = new CategoryRepository(GetCategoryContextMock(),
                                                                            GetCategoryPublisherMock());

            User user = User.Constructor(Guid.NewGuid());

            Category category = Category.New(user.Id.ToString());

            //Act & Assert

            await Assert.ThrowsAsync<DbUpdateException>(async () => await categoryRepository.Create(user.Id, category));
        }

        #endregion

        #region Update

        [Fact]
        public async void CategoryRepository_Update_UserExistAndCategoryCollectionIsNotEmpty_ShouldReturnSuccessTrue()
        {
            //Arrage

            User user = User.Constructor(Guid.NewGuid());

            Category[] categories =
            {
                Category.Constructor(Guid.NewGuid(), user.Id.ToString()),
                Category.Constructor(Guid.NewGuid(), user.Id.ToString()),
                Category.Constructor(Guid.NewGuid(), user.Id.ToString()),
            };

            ICategoryRepository categoryRepository = new CategoryRepository(GetCategoryContextMock(() => new Dictionary<User, IEnumerable<Category>>()
                                                                                                           {
                                                                                                                {user, categories}
                                                                                                           }),
                                                                            GetCategoryPublisherMock());

            string updatedCategoryName = "Updated category name";

            Category category = Category.Constructor(categories.First().Id, updatedCategoryName);

            //Act

            Result updateResult = await categoryRepository.Update(user.Id, category);

            //Assert

            Assert.True(updateResult.Success);
        }

        [Fact]
        public async void CategoryRepository_Update_UserExistAndCategoryCollectionIsEmpty_ShouldReturnSuccessFalse()
        {
            //Arrage

            User user = User.Constructor(Guid.NewGuid());

            ICategoryRepository categoryRepository = new CategoryRepository(GetCategoryContextMock(() => new Dictionary<User, IEnumerable<Category>>()
                                                                                                           {
                                                                                                                {user, Enumerable.Empty<Category>()}
                                                                                                           }),
                                                                            GetCategoryPublisherMock());

            Category category = Category.Constructor(Guid.NewGuid(), user.Id.ToString());

            //Act

            Result updateResult = await categoryRepository.Update(user.Id, category);

            //Assert

            Assert.False(updateResult.Success);
        }

        [Fact]
        public async void CategoryRepository_Update_UserNotExistAndCategoryCollectionIsEmpty_ShouldReturnSuccessFalse()
        {
            //Arrage

            ICategoryRepository categoryRepository = new CategoryRepository(GetCategoryContextMock(),
                                                                            GetCategoryPublisherMock());

            User user = User.Constructor(Guid.NewGuid());

            Category category = Category.Constructor(Guid.NewGuid(), user.Id.ToString());

            //Act

            Result updateResult = await categoryRepository.Update(user.Id, category);

            //Assert

            Assert.False(updateResult.Success);
        }

        #endregion

        #region Delete

        [Fact]
        public async void CategoryRepository_Delete_UserExistAndCategoryCollectionIsNotEmpty_ShouldReturnSuccessTrue()
        {
            //Arrage

            User user = User.Constructor(Guid.NewGuid());

            Category[] categories =
            {
                Category.Constructor(Guid.NewGuid(), user.Id.ToString()),
                Category.Constructor(Guid.NewGuid(), user.Id.ToString()),
                Category.Constructor(Guid.NewGuid(), user.Id.ToString()),
            };

            ICategoryRepository categoryRepository = new CategoryRepository(GetCategoryContextMock(() => new Dictionary<User, IEnumerable<Category>>()
                                                                                                           {
                                                                                                                {user, categories}
                                                                                                           }),
                                                                            GetCategoryPublisherMock());

            Category category = categories.First();

            //Act

            Result deleteResult = await categoryRepository.Delete(user.Id, category.Id);

            //Assert

            Assert.True(deleteResult.Success);
        }

        [Fact]
        public async void CategoryRepository_Delete_UserExistAndCategoryCollectionIsEmpty_ShouldReturnSuccessFalse()
        {
            //Arrage

            User user = User.Constructor(Guid.NewGuid());

            ICategoryRepository categoryRepository = new CategoryRepository(GetCategoryContextMock(() => new Dictionary<User, IEnumerable<Category>>()
                                                                                                           {
                                                                                                                {user, Enumerable.Empty<Category>()}
                                                                                                           }),
                                                                            GetCategoryPublisherMock());

            Category category = Category.Constructor(Guid.NewGuid(), user.Id.ToString());

            //Act

            Result deleteResult = await categoryRepository.Delete(user.Id, category.Id);

            //Assert

            Assert.False(deleteResult.Success);
        }

        [Fact]
        public async void CategoryRepository_Delete_UserNotExistAndCategoryCollectionIsEmpty_ShouldReturnSuccessFalse()
        {
            //Arrage

            ICategoryRepository categoryRepository = new CategoryRepository(GetCategoryContextMock(),
                                                                            GetCategoryPublisherMock());

            User user = User.Constructor(Guid.NewGuid());

            Category category = Category.Constructor(Guid.NewGuid(), user.Id.ToString());

            //Act

            Result deleteResult = await categoryRepository.Delete(user.Id, category.Id);

            //Assert

            Assert.False(deleteResult.Success);
        }

        [Fact]
        public async void CategoryRepository_Delete_UserExistAndCategoryCollectionIsNotEmptyPublisherIsDisabled_ShouldReturnSuccessFalse()
        {
            //Arrage

            User user = User.Constructor(Guid.NewGuid());

            Category[] categories =
            {
                Category.Constructor(Guid.NewGuid(), user.Id.ToString()),
                Category.Constructor(Guid.NewGuid(), user.Id.ToString()),
                Category.Constructor(Guid.NewGuid(), user.Id.ToString()),
            };

            ICategoryRepository categoryRepository = new CategoryRepository(GetCategoryContextMock(() => new Dictionary<User, IEnumerable<Category>>()
                                                                                                           {
                                                                                                                {user, categories}
                                                                                                           }),
                                                                            GetCategoryPublisherMock(mock =>
                                                                            {
                                                                                //Имитация недоступности брокера сообщений
                                                                                mock.Setup(_ => _.Delete(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(async () =>
                                                                                    Result.Failure(Errors.IsInternalServer("The publisher service is unavalible.")));
                                                                            }));

            Category category = categories.First();

            //Act

            Result deleteResult = await categoryRepository.Delete(user.Id, category.Id);

            //Assert

            Assert.False(deleteResult.Success);
        }


        #endregion
    }
}
