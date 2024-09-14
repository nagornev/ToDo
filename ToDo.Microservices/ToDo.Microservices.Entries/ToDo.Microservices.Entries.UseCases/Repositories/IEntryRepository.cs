using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDo.Microservices.Entries.Domain.Models;

namespace ToDo.Microservices.Entries.UseCases.Repositories
{
    public interface IEntryRepository
    {
        Task<IEnumerable<Entry>> Get();

        Task<Entry> Get(Guid entryId);

        Task<bool> Create(Entry entry);

        Task<bool> Update(Entry entry);

        Task<bool> Delete(Guid entryId);
    }
}
