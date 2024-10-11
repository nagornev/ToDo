using Microsoft.Extensions.Logging;
using ToDo.Domain.Results;
using ToDo.Microservices.Categories.Domain.Models;
using ToDo.Microservices.Categories.UseCases.Services;
using ToDo.Microservices.MQ.Models;
using ToDo.Microservices.MQ.Queries.GetCategory;
using ToDo.MQ.Abstractions;
using ToDo.MQ.Abstractions.Extensions;

namespace ToDo.Microservices.Categories.Infrastructure.Consumers
{
    public class GetCategoryConsumer : IMessageQueueConsumer
    {
        private ICategoryService _categoryService;

        private ILogger<GetCategoryConsumer> _logger;

        public GetCategoryConsumer(ICategoryService categoryService,
                                   ILogger<GetCategoryConsumer> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        public async Task Consume(IMessageQueueConsumerContext context)
        {
            GetCategoryProcedureRequest request = context.GetMessage<GetCategoryProcedureRequest>();

            try
            {

                Result<Category> categoryResult = await _categoryService.GetCategory(request.UserId, request.CategoryId);

                GetCategoryProcedureResponse response = new GetCategoryProcedureResponse(categoryResult.Success,
                                                                                         categoryResult.Success ?
                                                                                                new CategoryMQ(categoryResult.Content.Id, categoryResult.Content.Name) :
                                                                                                null,
                                                                                         categoryResult.Error);

                context.Respond(response);
            }
            catch (Exception exception)
            {
                context.Respond(GetCategoryProcedureResponse.Failure(Errors.IsInternalServer($"The '{nameof(Categories)}' service is unavailable.")));
                _logger.LogError(exception, "Invalid RPC (GetCategory) call.");
            }

            context.Ack();
        }
    }
}
