using ToDo.Domain.Results;
using ToDo.Microservices.Entries.Domain.Models;
using ToDo.Microservices.Entries.UseCases.Repositories;
using ToDo.Microservices.MQ.Queries.GetCategories;
using ToDo.Microservices.MQ.Queries.GetCategory;
using ToDo.MQ.Abstractions;
using ToDo.MQ.Abstractions.Extensions;

namespace ToDo.Microservices.Entries.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private IMessageQueueProcedureClient _procedureClient;

        public CategoryRepository(IMessageQueueProcedureClient procedureClient)
        {
            _procedureClient = procedureClient;
        }

        public async Task<Result<IEnumerable<Category>>> Get(Guid userId)
        {
            try
            {
                GetCategoriesProcedureResponse response = await _procedureClient.Send<GetCategoriesProcedureResponse, GetCategoriesProcedureRequest>(new GetCategoriesProcedureRequest(userId));

                return response.Success ?
                          Result<IEnumerable<Category>>.Successful(response.Content.Select(x => new Category(x.Id, x.Name))) :
                          Result<IEnumerable<Category>>.Failure(response.Error);
            }
            catch (TimeoutException)
            {
                return Result<IEnumerable<Category>>.Failure(error => error.InternalServer("The category service is unavaliable."));
            }
        }

        public async Task<Result<Category>> Get(Guid userId, Guid categoryId)
        {
            try
            {
                GetCategoryProcedureResponse response = await _procedureClient.Send<GetCategoryProcedureResponse, GetCategoryProcedureRequest>(new GetCategoryProcedureRequest(userId, categoryId));

                return response.Success ?
                          Result<Category>.Successful(new Category(response.Content.Id, response.Content.Name)) :
                          Result<Category>.Failure(response.Error);

            }
            catch (TimeoutException)
            {
                return Result<Category>.Failure(error => error.InternalServer("The category service is unavaliable."));
            }
        }
    }
}
