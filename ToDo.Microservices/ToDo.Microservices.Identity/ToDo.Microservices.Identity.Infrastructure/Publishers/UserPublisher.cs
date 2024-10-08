using ToDo.Domain.Results;
using ToDo.Microservices.Identity.Domain.Models;
using ToDo.Microservices.Identity.UseCases.Publishers;
using ToDo.Microservices.MQ.Models;
using ToDo.Microservices.MQ.Publishers;
using ToDo.MQ.Abstractions;

namespace ToDo.Microservices.Identity.Infrastructure.Publishers
{
    public class UserPublisher : IUserPublisher
    {
        private IMessageQueuePublishClient _publishClient;

        public UserPublisher(IMessageQueuePublishClient publishClient)
        {
            _publishClient = publishClient;
        }

        public async Task<Result> New(User user)
        {
            try
            {
                await _publishClient.Publish(new NewUserPublishMessage(new UserMQ(user.Id)));

                return Result.Successful();
            }
            catch (Exception exception)
            {
                return Result.Failure(Errors.IsMessage($"Unknown error. {exception.Message}"));
            }
        }
    }
}
