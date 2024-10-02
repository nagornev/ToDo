using System.Net.Http.Headers;
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

                return response.Result.Success ?
                          Result<IEnumerable<Category>>.Successful(response.Result.Content.Select(x => new Category(x.Id, x.Name))) :
                          Result<IEnumerable<Category>>.Failure(response.Result.Error);
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

                return response.Result.Success ?
                          Result<Category>.Successful(new Category(response.Result.Content.Id, response.Result.Content.Name)) :
                          Result<Category>.Failure(response.Result.Error);

            }
            catch (TimeoutException)
            {
                return Result<Category>.Failure(Errors.IsInternalServer("The category service is not avaliable."));
            }
        }
    }
}
