using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDo.Microservices.Entries.Domain.Models;

namespace ToDo.Microservices.Entries.UseCases.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> Get();

        Task<Category> Get(Guid categoryId);
    }
}
