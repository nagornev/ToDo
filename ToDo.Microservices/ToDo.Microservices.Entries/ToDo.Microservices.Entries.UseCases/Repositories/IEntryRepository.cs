using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDo.Microservices.Entries.Domain.Models;

namespace ToDo.Microservices.Entries.UseCases.Repositories
{
    public interface IEntryRepository
    {
        Task<IEnumerable<Entry>> Get(Guid userId);

        Task<Entry> Get(Guid userId, Guid entryId);

        Task<bool> Create(Guid userId, Entry entry);

        Task<bool> Update(Guid userId, Entry entry);

        Task<bool> Delete(Guid userId, Guid entryId);
    }
}
