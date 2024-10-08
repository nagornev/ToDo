using Microsoft.Extensions.Logging;
using ToDo.Domain.Results;
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

                IEnumerable<CategoryMQ> mqCategories = categoriesResult.Success ?
                                                            categoriesResult.Content.Select(x => new CategoryMQ(x.Id, x.Name)) :
                                                            Enumerable.Empty<CategoryMQ>();

                GetCategoriesProcedureResponse response = new GetCategoriesProcedureResponse(categoriesResult.Success ?
                                                                                    Result<IEnumerable<CategoryMQ>>.Successful(mqCategories) :
                                                                                    Result<IEnumerable<CategoryMQ>>.Failure(categoriesResult.Error));

                context.Respond(response);
            }
            catch (Exception exception)
            {
                context.Respond(new GetCategoriesProcedureResponse(Result<IEnumerable<CategoryMQ>>.Failure(Errors.IsInternalServer("The category service is not available."))));
                _logger.LogError(exception, "Invalid RPC (GetCategories) call.");
            }

            context.Ack();
        }
    }
}
