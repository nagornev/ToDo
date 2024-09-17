using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDo.Domain.Results;
using ToDo.Microservices.Entries.Domain.Collectings;

namespace ToDo.Microservices.Entries.UseCases.Services
{
    public interface IEntryService
    {
        Task<Result<IEnumerable<EntryCompose>>> GetEntries(Guid userId);

        Task<Result<EntryCompose>> GetEntry(Guid userId, Guid entryId);

        Task<Result> CreateEntry(Guid userId, Guid categoryId, string text, DateTime? deadline);

        Task<Result> UpdateEntry(Guid userId, Guid entryId, Guid categoryId, string text, DateTime? deadline, bool completed);

        Task<Result> DeleteEntry(Guid userId, Guid entryId);
    }
}
