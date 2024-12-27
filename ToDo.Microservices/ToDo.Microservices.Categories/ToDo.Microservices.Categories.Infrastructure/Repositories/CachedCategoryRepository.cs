using ToDo.Domain.Results;
using ToDo.Microservices.Categories.Domain.Models;
using ToDo.Microservices.Categories.Infrastructure.Cachers;
using ToDo.Microservices.Categories.UseCases.Repositories;
using ToDo.Cache.Abstractions.Extensions;

namespace ToDo.Microservices.Categories.Infrastructure.Repositories
{
    public class CachedCategoryRepository :  ICategoryRepository
    {
        private CategoryRepository _categoryRepository;

        private CategoryCacheIO _categoryCacheIO;

        public CachedCategoryRepository(CategoryRepository categoryRepository,
                                        CategoryCacheIO categoryCacheIO)
        {
            _categoryRepository = categoryRepository;
            _categoryCacheIO = categoryCacheIO;
        }

        public async Task<Result<IEnumerable<Category>>> Get(Guid userId)
        {
            Result<IEnumerable<Category>> categoriesResult = await _categoryCacheIO.Get(userId);

            if (!categoriesResult.Success)
                categoriesResult = await _categoryRepository.Get(userId);

            await _categoryCacheIO.Set(userId, categoriesResult);

            return categoriesResult;
        }

        public async Task<Result<Category>> Get(Guid userId, Guid categoryId)
        {
            Result<Category> categoryResult = await _categoryCacheIO.Get(userId,
                                                                         category => category.Id == categoryId,
                                                                         $"The category ({categoryId}) was not found.");

            if (!categoryResult.Success)
                categoryResult = await _categoryRepository.Get(userId, categoryId);

            return categoryResult;
        }


        public async Task<Result> Create(Guid userId, Category category)
        {
            Result creationResult = await _categoryRepository.Create(userId, category);

            if (creationResult.Success)
                await _categoryCacheIO.Remove(userId);

            return creationResult;
        }


        public async Task<Result> Update(Guid userId, Category category)
        {
            Result updateResult =  await _categoryRepository.Update(userId, category);

            if (updateResult.Success)
                await _categoryCacheIO.Remove(userId);

            return updateResult;
        }

        public async Task<Result> Delete(Guid userId, Guid categoryId)
        {
            Result deleteResult = await _categoryRepository.Delete(userId, categoryId);

            if(deleteResult.Success)
                await _categoryCacheIO.Remove(userId);

            return deleteResult;
        }
    }
}
