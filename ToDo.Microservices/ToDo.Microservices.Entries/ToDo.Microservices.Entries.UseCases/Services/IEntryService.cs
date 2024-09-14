using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDo.Domain.Results;
using ToDo.Microservices.Entries.Domain.Models;

namespace ToDo.Microservices.Entries.UseCases.Services
{
    public interface IEntryService
    {
        Task<Result<IEnumerable<Entry>>> GetEntries();

        Task<Result<Entry>> GetEntry(Guid entryId);

        Task<Result> CreateEntry(Guid categoryId, string text, DateTime? deadline);

        Task<Result> UpdateEntry(Guid entryId, Guid categoryId, string text, DateTime? deadline, bool completed);

        Task<Result> DeleteEntry(Guid entryId);
    }
}
