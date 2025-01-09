using Moq;
using ToDo.Domain.Results;
using ToDo.Microservices.Identity.Domain.Models;
using ToDo.Microservices.Identity.UseCases.Publishers;

namespace ToDo.Microservices.Identity.Tests.Mock
{
    public class UserPublisherMock : IUserPublisher
    {
        private Mock<IUserPublisher> _mock;

        public UserPublisherMock(Action<Mock<IUserPublisher>> configure = default)
        {
            _mock = new Mock<IUserPublisher>();

            configure?.Invoke(_mock);
        }

        public async Task<Result> New(User user)
        {
            return await _mock.Object.New(user);
        }
    }
}
