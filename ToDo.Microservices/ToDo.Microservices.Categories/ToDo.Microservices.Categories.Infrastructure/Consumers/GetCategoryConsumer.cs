using ToDo.Domain.Results;
using ToDo.Microservices.Categories.Domain.Models;
using ToDo.Microservices.Categories.UseCases.Services;
using ToDo.Microservices.MQ.Models;
using ToDo.Microservices.MQ.RPCs.GetCategory;
using ToDo.MQ.Abstractions;
using ToDo.MQ.Abstractions.Extensions;

namespace ToDo.Microservices.Categories.Infrastructure.Consumers
{
    public class GetCategoryConsumer : IMessageQueueConsumer
    {
        private ICategoryService _categoryService;

        public GetCategoryConsumer(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task Consume(IMessageQueueConsumerContext context)
        {
            GetCategoryRpcRequest request = context.GetMessage<GetCategoryRpcRequest>();

            Result<Category> categoryResult = await _categoryService.GetCategory(request.UserId, request.CategoryId);

            GetCategoryRpcResponse response = new GetCategoryRpcResponse(categoryResult.Success ?
                                                                            Result<CategoryMQ>.Successful(new CategoryMQ(categoryResult.Content.Id, categoryResult.Content.Name)) :
                                                                            Result<CategoryMQ>.Failure(categoryResult.Error));

            context.Respond(response);

            context.Ack();
        }
    }
}
