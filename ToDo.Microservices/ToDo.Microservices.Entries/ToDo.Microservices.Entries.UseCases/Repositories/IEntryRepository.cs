using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDo.Domain.Results;
using ToDo.Microservices.Entries.Domain.Models;

namespace ToDo.Microservices.Entries.UseCases.Repositories
{
    public interface IEntryRepository
    {
        Task<Result<IEnumerable<Entry>>> Get(Guid userId);

        Task<Result<Entry>> Get(Guid userId, Guid entryId);

        Task<Result> Create(Guid userId, Entry entry);

        Task<Result> Update(Guid userId, Entry entry);

        Task<Result> Delete(Guid userId, Guid entryId);

        Task<Result> DeleteByCategory(Guid userId, Guid categoryId);
    }
}
