using System.Net.Http.Headers;
using ToDo.Domain.Results;
using ToDo.Microservices.Entries.Domain.Models;
using ToDo.Microservices.Entries.UseCases.Services;
using ToDo.Microservices.MQ.Queries.GetCategories;
using ToDo.Microservices.MQ.Queries.GetCategory;
using ToDo.MQ.Abstractions;
using ToDo.MQ.Abstractions.Extensions;

namespace ToDo.Microservices.Entries.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private IMessageQueueProcedureClient _procedureClient;

        public CategoryService(IMessageQueueProcedureClient procedureClient)
        {
            _procedureClient = procedureClient;
        }

        public async Task<Result<IEnumerable<Category>>> Get(Guid userId)
        {
            try
            {
                GetCategoriesProcedureResponse response = await _procedureClient.Send<GetCategoriesProcedureResponse, GetCategoriesProcedureRequest>(new GetCategoriesProcedureRequest(userId));

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
                GetCategoryProcedureResponse response = await _procedureClient.Send<GetCategoryProcedureResponse, GetCategoryProcedureRequest>(new GetCategoryProcedureRequest(userId, categoryId));

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
