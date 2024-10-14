using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ToDo.Domain.Results;
using ToDo.Microservices.Entries.Domain.Models;

namespace ToDo.Microservices.Entries.UseCases.Repositories
{
    public interface ICategoryRepository
    {
        Task<Result<IEnumerable<Category>>> Get(Guid userId);

        Task<Result<Category>> Get(Guid userId, Guid categoryId);
    }
}
