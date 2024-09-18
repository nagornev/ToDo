using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ToDo.Domain.Results;
using ToDo.Microservices.Categories.Domain.Models;

namespace ToDo.Microservices.Categories.UseCases.Services
{
    public interface ICategoryRepository
    {
        Task<Result<IEnumerable<Category>>> GetCategories(Guid userId);

        Task<Result<Category>> GetCategory(Guid userId, Guid categoryId);

        Task<Result> CreateCategory(Guid userId, string name);

        Task<Result> UpdateCategory(Guid userId, Guid categoryId, string name);

        Task<Result> DeleteCategory(Guid userId, Guid categoryId);
    }
}
