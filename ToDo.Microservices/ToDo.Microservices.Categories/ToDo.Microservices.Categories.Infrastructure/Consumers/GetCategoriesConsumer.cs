using Microsoft.Extensions.Logging;
using ToDo.Domain.Results;
using ToDo.Domain.Results.Extensions;
using ToDo.Microservices.Categories.Domain.Models;
using ToDo.Microservices.Categories.UseCases.Services;
using ToDo.Microservices.MQ.Models;
using ToDo.Microservices.MQ.Queries.GetCategories;
using ToDo.MQ.Abstractions;
using ToDo.MQ.Abstractions.Extensions;

namespace ToDo.Microservices.Categories.Infrastructure.Consumers
{
    public class GetCategoriesConsumer : IMessageQueueConsumer
    {
        private ICategoryService _categoryService;

        private ILogger<GetCategoriesConsumer> _logger;

        public GetCategoriesConsumer(ICategoryService categoryService,
                                     ILogger<GetCategoriesConsumer> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        public async Task Consume(IMessageQueueConsumerContext context)
        {
            GetCategoriesProcedureRequest request = context.GetMessage<GetCategoriesProcedureRequest>();

            try
            {
                Result<IEnumerable<Category>> categoriesResult = await _categoryService.GetCategories(request.UserId);

                GetCategoriesProcedureResponse response = new GetCategoriesProcedureResponse(categoriesResult.Success,
                                                                                             categoriesResult.Content?.Select(x => new CategoryMQ(x.Id, x.Name)) ?? default,
                                                                                             categoriesResult.Error);

                context.Respond(response);
            }
            catch (Exception exception)
            {
                context.Respond(GetCategoriesProcedureResponse.Failure(error => error.InternalServer($"The category service is unavailable.")));

                _logger.LogError(exception, "Invalid RPC (GetCategories) call.");
            }

            context.Ack();
        }
    }
}
