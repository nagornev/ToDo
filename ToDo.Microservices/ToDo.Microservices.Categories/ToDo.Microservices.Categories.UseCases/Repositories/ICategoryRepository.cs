using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDo.Microservices.Categories.Domain.Models;

namespace ToDo.Microservices.Categories.UseCases.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> Get(Guid userId);

        Task<Category> Get(Guid userId, Guid categoryId);

        Task<bool> Create(Guid userId, Category category);

        Task<bool> Update(Guid userId, Category category);

        Task<bool> Delete(Guid userId, Guid categoryId);
    }
}
