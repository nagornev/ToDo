using Moq;
using ToDo.Domain.Results;
using ToDo.Microservices.Entries.Domain.Models;
using ToDo.Microservices.Entries.UseCases.Services;

namespace ToDo.Microservices.Entries.Tests.Mock
{
    public class CategoryServiceMock : ICategoryService
    {
        private Mock<ICategoryService> _categoryServiceMock;

        public CategoryServiceMock(Action<Mock<ICategoryService>> configure)
        {
            _categoryServiceMock = new Mock<ICategoryService>();

            configure.Invoke(_categoryServiceMock);
        }

        public async Task<Result<IEnumerable<Category>>> GetCategories(Guid userId)
        {
            return await _categoryServiceMock.Object.GetCategories(userId);
        }

        public async Task<Result<Category>> GetCategory(Guid userId, Guid categoryId)
        {
            return await _categoryServiceMock.Object.GetCategory(userId, categoryId);
        }
    }
}
