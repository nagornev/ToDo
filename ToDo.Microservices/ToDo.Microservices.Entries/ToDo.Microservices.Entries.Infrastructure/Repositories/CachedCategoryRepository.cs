using ToDo.Cache.Abstractions.Extensions;
using ToDo.Domain.Results;
using ToDo.Microservices.Entries.Domain.Models;
using ToDo.Microservices.Entries.UseCases.Caches;
using ToDo.Microservices.Entries.UseCases.Repositories;

namespace ToDo.Microservices.Entries.Infrastructure.Repositories
{
    public class CachedCategoryRepository : ICategoryRepository
    {
        private CategoryRepository _categoryRepository;

        private ICategoryCacheReader _categoryCacheReader;

        public CachedCategoryRepository(CategoryRepository categoryRepository,
                                        ICategoryCacheReader categoryCacheReader)
        {
            _categoryRepository = categoryRepository;
            _categoryCacheReader = categoryCacheReader;
        }

        public async Task<Result<IEnumerable<Category>>> Get(Guid userId)
        {
            Result<IEnumerable<Category>> categoriesResult = await _categoryCacheReader.Get(userId);

            if (!categoriesResult.Success)
                categoriesResult = await _categoryRepository.Get(userId);

            return categoriesResult;
        }

        public async Task<Result<Category>> Get(Guid userId, Guid categoryId)
        {
            Result<Category> categoryResult = await _categoryCacheReader.Get(userId,
                                                                             category => category.Id == categoryId,
                                                                             $"The categoty ({categoryId}) was not found.");

            if (!categoryResult.Success)
                categoryResult = await _categoryRepository.Get(userId, categoryId);

            return categoryResult;
        }
    }
}
