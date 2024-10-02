using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDo.Domain.Results;
using ToDo.Microservices.Categories.Domain.Models;

namespace ToDo.Microservices.Categories.UseCases.Repositories
{
    public interface ICategoryRepository
    {
        Task<Result<IEnumerable<Category>>> Get(Guid userId);

        Task<Result<Category>> Get(Guid userId, Guid categoryId);

        Task<Result> Create(Guid userId, Category category);

        Task<Result> Update(Guid userId, Category category);

        Task<Result> Delete(Guid userId, Guid categoryId);
    }
}
