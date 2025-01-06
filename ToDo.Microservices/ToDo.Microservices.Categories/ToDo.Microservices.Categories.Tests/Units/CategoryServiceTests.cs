using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Domain.Results;
using ToDo.Microservices.Categories.Domain.Models;
using ToDo.Microservices.Categories.Infrastructure.Repositories;
using ToDo.Microservices.Categories.Infrastructure.Services;
using ToDo.Microservices.Categories.Tests.Mock;
using ToDo.Microservices.Categories.UseCases.Publishers;
using ToDo.Microservices.Categories.UseCases.Repositories;
using ToDo.Microservices.Categories.UseCases.Services;

namespace ToDo.Microservices.Categories.Tests.Units
{
    public class CategoryServiceTests
    {
        private CategoryContextMock GetCategoryContextMock(Func<IReadOnlyDictionary<User, IEnumerable<Category>>> startContext = null)
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

        #region GetCategories and GetCategory

        [Fact]
        public async void CategoryService_GetCategories_UserExistAndCategoryCollectionIsNotEmpty_ShouldReturnSuccessTrueAndCategoryCollection()
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

            ICategoryService categoryService = new CategoryService(categoryRepository);

            //Act

            Result<IEnumerable<Category>> categoriesResult = await categoryService.GetCategories(user.Id);

            //Assert

            Assert.True(categoriesResult.Success);
            Assert.NotEmpty(categoriesResult.Content);
            Assert.All(categoriesResult.Content, (category) => Assert.Contains(user.Id.ToString(), category.Name));
        }

        [Fact]
        public async void CategoryService_GetCategories_UserExistAndCategoryCollectionIsEmpty_ShouldReturnSuccessTrueAndEmptyCategoryCollection()
        {
            //Arrage

            User user = User.Constructor(Guid.NewGuid());

            ICategoryRepository categoryRepository = new CategoryRepository(GetCategoryContextMock(() => new Dictionary<User, IEnumerable<Category>>()
                                                                                                           {
                                                                                                                {user, Enumerable.Empty<Category>()}
                                                                                                           }),
                                                                            GetCategoryPublisherMock());

            ICategoryService categoryService = new CategoryService(categoryRepository);

            //Act

            Result<IEnumerable<Category>> categoriesResult = await categoryService.GetCategories(user.Id);

            //Assert

            Assert.True(categoriesResult.Success);
            Assert.Empty(categoriesResult.Content);
        }

        [Fact]
        public async void CategoryService_GetCategories_UserNotExistAndCategoryCollectionIsEmpty_ShouldReturnSuccessFalse()
        {
            //Arrage

            ICategoryRepository categoryRepository = new CategoryRepository(GetCategoryContextMock(),
                                                                            GetCategoryPublisherMock());

            ICategoryService categoryService = new CategoryService(categoryRepository);

            User user = User.Constructor(Guid.NewGuid());

            //Act

            Result<IEnumerable<Category>> categoriesResult = await categoryService.GetCategories(user.Id);

            //Assert

            Assert.False(categoriesResult.Success);
        }


        [Fact]
        public async void CategoryService_GetCategory_UserExistAndCategoryCollectionIsNotEmpty_ShouldReturnSuccessTrueAndCategory()
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

            ICategoryService categoryService = new CategoryService(categoryRepository);

            Category category = categories.First();

            //Act

            Result<Category> categoryResult = await categoryService.GetCategory(user.Id, category.Id);

            //Assert

            Assert.True(categoryResult.Success);
            Assert.Contains(user.Id.ToString(), category.Name);
        }

        [Fact]
        public async void CategoryService_GetCategory_UserExistAndCategoryCollectionIsEmpty_ShouldReturnSuccessFalse()
        {
            //Arrage

            User user = User.Constructor(Guid.NewGuid());

            ICategoryRepository categoryRepository = new CategoryRepository(GetCategoryContextMock(() => new Dictionary<User, IEnumerable<Category>>()
                                                                                                           {
                                                                                                                {user, Enumerable.Empty<Category>()}
                                                                                                           }),
                                                                            GetCategoryPublisherMock());

            ICategoryService categoryService = new CategoryService(categoryRepository);

            Category category = Category.Constructor(Guid.NewGuid(), user.Id.ToString());

            //Act

            Result<Category> categoryResult = await categoryService.GetCategory(user.Id, category.Id);

            //Assert

            Assert.False(categoryResult.Success);
        }

        [Fact]
        public async void CategoryService_GetCategory_UserNotExistAndCategoryCollectionIsNotEmpty_ShouldReturnSuccessFalse()
        {
            //Arrage

            ICategoryRepository categoryRepository = new CategoryRepository(GetCategoryContextMock(),
                                                                            GetCategoryPublisherMock());

            ICategoryService categoryService = new CategoryService(categoryRepository);

            User user = User.Constructor(Guid.NewGuid());

            Category category = Category.Constructor(Guid.NewGuid(), user.Id.ToString());

            //Act

            Result<Category> categoryResult = await categoryService.GetCategory(user.Id, category.Id);

            //Assert

            Assert.False(categoryResult.Success);
        }

        #endregion

        #region CreateCategory

        [Fact]
        public async void CategoryService_CreateCategory_UserExistAndCategoryCollectionIsNotEmpty_ShouldReturnSuccessTrue()
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

            ICategoryService categoryService = new CategoryService(categoryRepository);

            //Act

            Result createResult = await categoryService.CreateCategory(user.Id, user.Id.ToString());

            //Assert

            Assert.True(createResult.Success);
        }

        [Fact]
        public async void CategoryService_CreateCategory_UserExistAndCategoryCollectionIsEmpty_ShouldReturnSuccessTrue()
        {
            //Arrage

            User user = User.Constructor(Guid.NewGuid());

            ICategoryRepository categoryRepository = new CategoryRepository(GetCategoryContextMock(() => new Dictionary<User, IEnumerable<Category>>()
                                                                                                           {
                                                                                                                {user, Enumerable.Empty<Category>()}
                                                                                                           }),
                                                                            GetCategoryPublisherMock());

            ICategoryService categoryService = new CategoryService(categoryRepository);

            //Act

            Result createResult = await categoryService.CreateCategory(user.Id, user.Id.ToString());

            //Assert

            Assert.True(createResult.Success);
        }


        [Fact]
        public async void CategoryService_CreateCategory_UserNotExistAndCategoryCollectionIsEmpty_ShouldReturnSuccessTrue()
        {
            //Arrage

            ICategoryRepository categoryRepository = new CategoryRepository(GetCategoryContextMock(),
                                                                            GetCategoryPublisherMock());

            ICategoryService categoryService = new CategoryService(categoryRepository);

            User user = User.Constructor(Guid.NewGuid());

            //Act & Assert

            await Assert.ThrowsAsync<DbUpdateException>(async ()=> await categoryService.CreateCategory(user.Id, user.Id.ToString()));
        }

        #endregion

        #region UpdateCategory

        [Fact]
        public async void CategoryService_UpdateCategory_UserExistAndCategoryCollectionIsNotEmpty_ShouldReturnSuccessTrue()
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

            ICategoryService categoryService = new CategoryService(categoryRepository);

            string updatedCategoryName = "Updated name";

            Category category = Category.Constructor(categories.First().Id, updatedCategoryName);

            //Act

            Result updateResult = await categoryService.UpdateCategory(user.Id, category.Id, category.Name);

            //Assert

            Assert.True(updateResult.Success);
        }

        [Fact]
        public async void CategoryService_UpdateCategory_UserExistAndCategoryCollectionIsEmpty_ShouldReturnSuccessFalse()
        {
            //Arrage

            User user = User.Constructor(Guid.NewGuid());

            ICategoryRepository categoryRepository = new CategoryRepository(GetCategoryContextMock(() => new Dictionary<User, IEnumerable<Category>>()
                                                                                                           {
                                                                                                                {user, Enumerable.Empty<Category>()}
                                                                                                           }),
                                                                            GetCategoryPublisherMock());

            ICategoryService categoryService = new CategoryService(categoryRepository);

            string updatedCategoryName = "Updated name";

            Category category = Category.Constructor(Guid.NewGuid(), updatedCategoryName);

            //Act

            Result updateResult = await categoryService.UpdateCategory(user.Id, category.Id, category.Name);

            //Assert

            Assert.False(updateResult.Success);
        }

        [Fact]
        public async void CategoryService_UpdateCategory_UserNotExistAndCategoryCollectionIsEmpty_ShouldReturnSuccessFalse()
        {
            //Arrage

            ICategoryRepository categoryRepository = new CategoryRepository(GetCategoryContextMock(),
                                                                            GetCategoryPublisherMock());

            ICategoryService categoryService = new CategoryService(categoryRepository);


            User user = User.Constructor(Guid.NewGuid());

            string updatedCategoryName = "Updated name";

            Category category = Category.Constructor(Guid.NewGuid(), updatedCategoryName);

            //Act

            Result updateResult = await categoryService.UpdateCategory(user.Id, category.Id, category.Name);

            //Assert

            Assert.False(updateResult.Success);
        }


        #endregion

        #region DeleteCategory

        [Fact]
        public async void CategoryService_DeleteCategory_UserExistAndCategoryCollectionIsNotEmptyAndPublisherEnabled_ShouldReturnSuccessTrue()
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

            ICategoryService categoryService = new CategoryService(categoryRepository);


            Category category = categories.First();

            //Act

            Result deleteResult = await categoryService.DeleteCategory(user.Id, category.Id);

            //Assert

            Assert.True(deleteResult.Success);
        }

        [Fact]
        public async void CategoryService_DeleteCategory_UserExistAndCategoryCollectionIsEmptyAndPublisherEnabled_ShouldReturnSuccessFalse()
        {
            //Arrage

            User user = User.Constructor(Guid.NewGuid());

            ICategoryRepository categoryRepository = new CategoryRepository(GetCategoryContextMock(() => new Dictionary<User, IEnumerable<Category>>()
                                                                                                           {
                                                                                                                {user, Enumerable.Empty<Category>()}
                                                                                                           }),
                                                                            GetCategoryPublisherMock());

            ICategoryService categoryService = new CategoryService(categoryRepository);


            Category category = Category.New(user.Id.ToString());

            //Act

            Result deleteResult = await categoryService.DeleteCategory(user.Id, category.Id);

            //Assert

            Assert.False(deleteResult.Success);
        }

        [Fact]
        public async void CategoryService_DeleteCategory_UserNotExistAndCategoryCollectionIsEmptyAndPublisherEnabled_ShouldReturnSuccessFalse()
        {
            //Arrage

            ICategoryRepository categoryRepository = new CategoryRepository(GetCategoryContextMock(),
                                                                            GetCategoryPublisherMock());

            ICategoryService categoryService = new CategoryService(categoryRepository);

            User user = User.Constructor(Guid.NewGuid());

            Category category = Category.New(user.Id.ToString());

            //Act

            Result deleteResult = await categoryService.DeleteCategory(user.Id, category.Id);

            //Assert

            Assert.False(deleteResult.Success);
        }


        [Fact]
        public async void CategoryService_DeleteCategory_UserExistAndCategoryCollectionIsNotEmptyAndPublisherDisabled_ShouldReturnSuccessFalse()
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
                                                                            GetCategoryPublisherMock(mock=>
                                                                            {
                                                                                //Имитация недоступности брокера сообщений
                                                                                mock.Setup(_ => _.Delete(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(async () =>
                                                                                    Result.Failure(Errors.IsInternalServer("The publisher service is unavalible.")));
                                                                            }));

            ICategoryService categoryService = new CategoryService(categoryRepository);


            Category category = categories.First();

            //Act

            Result deleteResult = await categoryService.DeleteCategory(user.Id, category.Id);

            //Assert

            Assert.False(deleteResult.Success);
        }

        #endregion
    }
}
