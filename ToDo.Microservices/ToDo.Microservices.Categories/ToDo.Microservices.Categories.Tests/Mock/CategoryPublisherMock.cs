using Moq;
using ToDo.Domain.Results;
using ToDo.Microservices.Categories.UseCases.Publishers;

namespace ToDo.Microservices.Categories.Tests.Mock
{
    internal class CategoryPublisherMock : ICategoryPubliser
    {
        private Mock<ICategoryPubliser> _mock;

        public CategoryPublisherMock(Action<Mock<ICategoryPubliser>> configure)
        {
            if (configure is null)
                throw new ArgumentNullException(nameof(configure));

            _mock = new Mock<ICategoryPubliser>();

            configure.Invoke(_mock);
        }

        public async Task<Result> Delete(Guid userId, Guid categoryId)
        {
            return await _mock.Object.Delete(userId, categoryId);
        }
    }
}
