using ToDo.Domain.Results;
using ToDo.Microservices.Entries.Domain.Models;
using ToDo.Microservices.Entries.UseCases.Services;
using ToDo.Microservices.MQ.RPCs.GetCategories;
using ToDo.Microservices.MQ.RPCs.GetCategory;
using ToDo.MQ.Abstractions;
using ToDo.MQ.Abstractions.Extensions;

namespace ToDo.Microservices.Entries.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private IMessageQueueClient _messageQueue;

        public CategoryService(IMessageQueueClient messageQueue)
        {
            _messageQueue = messageQueue;
        }

        public async Task<Result<IEnumerable<Category>>> Get(Guid userId)
        {
            try
            {
                GetCategoriesRpcResponse response = await _messageQueue.Send<GetCategoriesRpcResponse, GetCategoriesRpcRequest>(new GetCategoriesRpcRequest(userId));

                IEnumerable<Category> categories = response.Categories.Select(x => new Category(x.Id, x.Name));

                return Result<IEnumerable<Category>>.Successful(categories);
            }
            catch (TimeoutException)
            {
                return Result<IEnumerable<Category>>.Failure(Errors.IsInternalServer("The category service is not avaliable."));
            }
        }

        public async Task<Result<Category>> Get(Guid userId, Guid categoryId)
        {
            try
            {
                GetCategoryRpcResponse response = await _messageQueue.Send<GetCategoryRpcResponse, GetCategoryRpcRequest>(new GetCategoryRpcRequest(userId, categoryId));

                Category? category = !(response.Category is null) ?
                                         new Category(response.Category.Id, response.Category.Name) :
                                         default;

                return !(category is null) ?
                           Result<Category>.Successful(category) :
                           Result<Category>.Failure(Errors.IsNull($"The category {categoryId} was not found."));

            }
            catch (TimeoutException)
            {
                return Result<Category>.Failure(Errors.IsInternalServer("The category service is not avaliable."));
            }
        }
    }
}
