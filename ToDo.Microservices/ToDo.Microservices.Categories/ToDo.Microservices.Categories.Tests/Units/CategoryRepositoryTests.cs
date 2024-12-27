using ToDo.Microservices.Categories.Infrastructure.Repositories;
using ToDo.Microservices.Categories.Tests.Mock;
using ToDo.Microservices.Categories.UseCases.Repositories;
using Moq;
using ToDo.Domain.Results;
using ToDo.Microservices.Categories.Domain.Models;

namespace ToDo.Microservices.Categories.Tests.Units
{
    public class CategoryRepositoryTests
    {
        private CategoryContextMock _categoryContextMock;

        private CategoryPublisherMock _categoryPublisherMock;

        private ICategoryRepository _categoryRepository;

        public CategoryRepositoryTests()
        {
            _categoryContextMock = new CategoryContextMock($"{Guid.NewGuid()}", () =>
            {
                return new Dictionary<User, IEnumerable<Category>>()
                {
                    { User.Constructor(Guid.NewGuid()), new Category[] 
                    { 
                            Category.Constructor(Guid.Parse("43b8f286-dc09-4768-b084-3d6bc78fe0b4"), "test category"), 
                            Category.New("test category") } 
                    } ,
                    { User.Constructor(Guid.NewGuid()), new Category[] { } } ,
                    { User.Constructor(Guid.NewGuid()), new Category[] { Category.New("test category") } } ,
                };
            });

            _categoryPublisherMock = new CategoryPublisherMock((mock)=>
            {
                mock.Setup(_ => _.Delete(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(async () => Result.Successful());
            });

            _categoryRepository = new CategoryRepository(_categoryContextMock, _categoryPublisherMock);
        }

        [Fact]
        public async void CategoryRepository_GetCategories_ShouldReturnSuccessTrueAndUserCategories()
        {
            //Arrage

            User user = _categoryContextMock.DefaultUsers.First();

            //Act

            Result<IEnumerable<Category>> categoriesResult = await _categoryRepository.Get(user.Id);

            //Assert

            Assert.True(categoriesResult.Success);
            Assert.NotEmpty(categoriesResult.Content);
            Assert.All(categoriesResult.Content, (category) => Assert.Contains(_categoryContextMock.Data[user], (selectedCategory)=>selectedCategory.Id == category.Id));
        }

        [Fact]
        public async void CategoryRepository_GetCategory_ShouldReturnSuccessTrueAndUserCategory()
        {
            
        }

        [Fact]
        public async void CategoryRepository_Create_ShouldReturnSuccessTrue()
        {
            //Arrage

            User user = _categoryContextMock.DefaultUsers.First();

            Category category = Category.New("new category");

            //Act

            Result createResult = await _categoryRepository.Create(user.Id, category);

            //Assert

            Assert.True(createResult.Success);
        }
    }
}
