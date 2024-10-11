using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDo.Domain.Results;
using ToDo.Microservices.Entries.Domain.Models;

namespace ToDo.Microservices.Entries.UseCases.Cachers
{
    public interface IEntryCacher
    {
        Task<Result<IEnumerable<Entry>>> Get(Guid userId);

        Task<Result> Set(Guid userId, Result<IEnumerable<Entry>> entriesResult);

        Task<Result> Remove(Guid userId);
    }
}
