using ToDo.Domain.Results;
using ToDo.Microservices.Categories.Domain.Models;
using ToDo.Microservices.Categories.UseCases.Services;
using ToDo.Microservices.MQ.Models;
using ToDo.Microservices.MQ.RPCs.GetCategories;
using ToDo.MQ.Abstractions;
using ToDo.MQ.Abstractions.Extensions;

namespace ToDo.Microservices.Categories.Infrastructure.Consumers
{
    public class GetCategoriesConsumer : IMessageQueueConsumer
    {
        private ICategoryService _categoryService;

        public GetCategoriesConsumer(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task Consume(IMessageQueueConsumerContext context)
        {
            GetCategoriesRpcRequest request = context.GetMessage<GetCategoriesRpcRequest>();

            Result<IEnumerable<Category>> categoriesResult = await _categoryService.GetCategories(request.UserId);

            IEnumerable<CategoryMQ> mqCategories = categoriesResult.Success ?
                                                        categoriesResult.Content.Select(x => new CategoryMQ(x.Id, x.Name)) :
                                                        Enumerable.Empty<CategoryMQ>();

            GetCategoriesRpcResponse response = new GetCategoriesRpcResponse(mqCategories.ToList());

            context.Respond(response);

            context.Ack();
        }
    }
}
