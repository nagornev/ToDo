using ToDo.Domain.Results;
using ToDo.Microservices.Identity.Domain.Models;
using ToDo.Microservices.Identity.UseCases.Producers;
using ToDo.Microservices.MQ.Publishers;
using ToDo.MQ.Abstractions;

namespace ToDo.Microservices.Identity.Infrastructure.Producers
{
    public class UserProducer : IUserProducer
    {
        private IMessageQueueClient _messageQueue;

        public UserProducer(IMessageQueueClient messageQueue)
        {
            _messageQueue = messageQueue;
        }

        public async Task<Result> New(User user)
        {
            try
            {
                await _messageQueue.Publish(new NewUserPublish(user.Id));

                return Result.Successful();
            }
            catch (Exception exception)
            {
                return Result.Failure(Errors.IsMessage($"Unknown error. {exception.Message}"));
            }
        }
    }
}
