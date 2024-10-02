using ToDo.Domain.Results;
using ToDo.Microservices.Categories.UseCases.Publishers;
using ToDo.Microservices.MQ.Publishers;
using ToDo.MQ.Abstractions;

namespace ToDo.Microservices.Categories.Infrastructure.Publishers
{
    public class CategoryPublisher : ICategoryPubliser
    {
        private IMessageQueueClient _messageQueue;

        public CategoryPublisher(IMessageQueueClient messageQueue)
        {
            _messageQueue = messageQueue;
        }

        public async Task<Result> Delete(Guid userId, Guid categoryId)
        {
            try
            {
                await _messageQueue.Publish(new DeleteCategoryPublish(userId, categoryId));

                return Result.Successful();
            }
            catch (Exception exception)
            {
                return Result.Failure(Errors.IsInternalServer(exception.StackTrace));
            }
        }
    }
}
