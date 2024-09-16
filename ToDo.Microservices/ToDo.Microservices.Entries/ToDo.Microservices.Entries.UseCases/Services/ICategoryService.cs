using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDo.Domain.Results;
using ToDo.Microservices.Entries.Domain.Models;

namespace ToDo.Microservices.Entries.UseCases.Services
{
    public interface ICategoryService
    {
        Task<Result<IEnumerable<Category>>> Get();

        Task<Result<Category>> Get(Guid categoryId);
    }
}
