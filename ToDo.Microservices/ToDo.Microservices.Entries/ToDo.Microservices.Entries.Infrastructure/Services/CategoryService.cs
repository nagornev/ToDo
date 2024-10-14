using ToDo.Domain.Results;
using ToDo.Microservices.Entries.Domain.Models;
using ToDo.Microservices.Entries.UseCases.Repositories;
using ToDo.Microservices.Entries.UseCases.Services;

namespace ToDo.Microservices.Entries.Infrastructure.Services
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
            return await _categoryRepository.Get(userId);
        }

        public async Task<Result<Category>> GetCategory(Guid userId, Guid categoryId)
        {
            return await _categoryRepository.Get(userId, categoryId);
        }
    }
}
