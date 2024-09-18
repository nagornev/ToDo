using ToDo.Domain.Results;
using ToDo.Microservices.Categories.Domain.Models;
using ToDo.Microservices.Categories.UseCases.Repositories;
using ToDo.Microservices.Categories.UseCases.Services;

namespace ToDo.Microservices.Categories.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Result<IEnumerable<Category>>> GetCategories(Guid userId)
        {
            IEnumerable<Category> categories = await _categoryRepository.Get(userId);

            return Result<IEnumerable<Category>>.Successful(categories);
        }

        public async Task<Result<Category>> GetCategory(Guid userId, Guid categoryId)
        {
            Category category = await _categoryRepository.Get(userId, categoryId);

            return category is not null ?
                    Result<Category>.Successful(category) :
                    Result<Category>.Failure(Errors.IsNull($"The category ({categoryId}) was not found."));
        }

        public async Task<Result> CreateCategory(Guid userId, string name)
        {
            Category category = Category.New(name);

            return await _categoryRepository.Create(userId, category) ?
                    Result.Successful() :
                    Result.Failure(Errors.IsMessage("The category was not created. Please check the entry parameters and try again later."));
        }


        public async Task<Result> UpdateCategory(Guid userId, Guid categoryId, string name)
        {
            Category category = Category.Constructor(categoryId, name);

            return await _categoryRepository.Update(userId, category) ?
                    Result.Successful() :
                    Result.Failure(Errors.IsNull($"The category ({categoryId}) was not found."));
        }

        public async Task<Result> DeleteCategory(Guid userId, Guid categoryId)
        {
            return await _categoryRepository.Delete(userId, categoryId) ?
                    Result.Successful() :
                    Result.Failure(Errors.IsNull($"The category ({categoryId}) was not found."));
        }



    }
}
