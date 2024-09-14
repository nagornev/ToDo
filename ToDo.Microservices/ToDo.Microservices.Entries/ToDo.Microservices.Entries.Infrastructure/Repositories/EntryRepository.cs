using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Microservices.Entries.Domain.Models;
using ToDo.Microservices.Entries.UseCases.Repositories;

namespace ToDo.Microservices.Entries.Infrastructure.Repositories
{
    public class EntryRepository : IEntryRepository
    {
        public Task<IEnumerable<Entry>> Get()
        {
            throw new NotImplementedException();
        }

        public Task<Entry> Get(Guid entryId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Create(Entry entry)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(Entry entry)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(Guid entryId)
        {
            throw new NotImplementedException();
        }
    }
}
