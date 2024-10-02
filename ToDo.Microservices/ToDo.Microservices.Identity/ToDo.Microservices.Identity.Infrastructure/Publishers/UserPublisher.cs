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
        private IMessageQueueClient _messageQueue;

        public UserPublisher(IMessageQueueClient messageQueue)
        {
            _messageQueue = messageQueue;
        }

        public async Task<Result> New(User user)
        {
            try
            {
                await _messageQueue.Publish(new NewUserPublish(new UserMQ(user.Id, user.Email)));

                return Result.Successful();
            }
            catch (Exception exception)
            {
                return Result.Failure(Errors.IsMessage($"Unknown error. {exception.Message}"));
            }
        }
    }
}
