using ToDo.Domain.Results;
using ToDo.Microservices.Categories.UseCases.Publishers;
using ToDo.Microservices.MQ.Publishers;
using ToDo.MQ.Abstractions;

namespace ToDo.Microservices.Categories.Infrastructure.Publishers
{
    public class CategoryPublisher : ICategoryPubliser
    {
        private IMessageQueuePublishClient _publishClient;

        public CategoryPublisher(IMessageQueuePublishClient publishClient)
        {
            _publishClient = publishClient;
        }

        public async Task<Result> Delete(Guid userId, Guid categoryId)
        {
            try
            {
                await _publishClient.Publish(new DeleteCategoryPublishMessage(userId, categoryId));

                return Result.Successful();
            }
            catch (Exception exception)
            {
                return Result.Failure(error => error.InternalServer("The category publisher is unavailable."));
            }
        }
    }
}
