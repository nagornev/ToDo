using ToDo.Domain.Results;
using ToDo.Cache.Abstractions.Extensions;
using ToDo.Microservices.Entries.Domain.Models;
using ToDo.Microservices.Entries.Infrastructure.Cachers;
using ToDo.Microservices.Entries.UseCases.Repositories;
using ToDo.MQ.Abstractions;

namespace ToDo.Microservices.Entries.Infrastructure.Repositories
{
    public class CachedCategoryRepository : CategoryRepository, ICategoryRepository
    {
        private CategoryCacheReader _categoryCacheReader;

        public CachedCategoryRepository(IMessageQueueProcedureClient procedureClient,
                                        CategoryCacheReader categoryCacheReader)
            : base(procedureClient)
        {
            _categoryCacheReader = categoryCacheReader;
        }

        public new async Task<Result<IEnumerable<Category>>> Get(Guid userId)
        {
            Result<IEnumerable<Category>> categoriesResult = await _categoryCacheReader.Get(userId);

            if(!categoriesResult.Success)
                categoriesResult = await base.Get(userId);

            return categoriesResult;
        }

        public new async Task<Result<Category>> Get(Guid userId, Guid categoryId)
        {
            Result<Category> categoryResult = await _categoryCacheReader.Get(userId,
                                                                             category => category.Id == categoryId,
                                                                             $"The categoty ({categoryId}) was not found.");

            if (!categoryResult.Success)
                categoryResult = await base.Get(userId, categoryId);

            return categoryResult;
        }
    }
}
