using ToDo.Domain.Results;
using ToDo.Microservices.Categories.Database.Contexts;
using ToDo.Microservices.Categories.Domain.Models;
using ToDo.Microservices.Categories.Infrastructure.Cachers;
using ToDo.Microservices.Categories.UseCases.Publishers;
using ToDo.Microservices.Categories.UseCases.Repositories;
using ToDo.Cache.Abstractions.Extensions;

namespace ToDo.Microservices.Categories.Infrastructure.Repositories
{
    public class CachedCategoryRepository : CategoryRepository, ICategoryRepository
    {
        private CategoryCacheIO _categoryCacheIO;

        public CachedCategoryRepository(CategoryContext context,
                                        ICategoryPubliser categoryPublisher,
                                        CategoryCacheIO categoryCacheIO) 
            : base(context, categoryPublisher)
        {
            _categoryCacheIO = categoryCacheIO;
        }

        public new async Task<Result<IEnumerable<Category>>> Get(Guid userId)
        {
            Result<IEnumerable<Category>> categoriesResult = await _categoryCacheIO.Get(userId);

            if (!categoriesResult.Success)
                categoriesResult = await base.Get(userId);

            await _categoryCacheIO.Set(userId, categoriesResult);

            return categoriesResult;
        }

        public new async Task<Result<Category>> Get(Guid userId, Guid categoryId)
        {
            Result<Category> categoryResult = await _categoryCacheIO.Get(userId,
                                                                         category => category.Id == categoryId,
                                                                         $"The category ({categoryId}) was not found.");

            if (!categoryResult.Success)
                categoryResult = await base.Get(userId, categoryId);

            return categoryResult;
        }


        public new async Task<Result> Create(Guid userId, Category category)
        {
            Result creationResult = await base.Create(userId, category);

            if (creationResult.Success)
                await _categoryCacheIO.Remove(userId);

            return creationResult;
        }


        public new async Task<Result> Update(Guid userId, Category category)
        {
            Result updateResult =  await base.Update(userId, category);

            if (!updateResult.Success)
                await _categoryCacheIO.Remove(userId);

            return updateResult;
        }

        public new async Task<Result> Delete(Guid userId, Guid categoryId)
        {
            Result deleteResult = await base.Delete(userId, categoryId);

            if(!deleteResult.Success)
                await _categoryCacheIO.Remove(userId);

            return deleteResult;
        }
    }
}
