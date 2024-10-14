using ToDo.Domain.Results;
using ToDo.Microservices.Entries.Domain.Models;
using ToDo.Microservices.Entries.Infrastructure.Cachers;
using ToDo.Microservices.Entries.UseCases.Repositories;
using ToDo.Cache.Abstractions.Extensions;
using ToDo.Microservices.Entries.Database.Contexts;

namespace ToDo.Microservices.Entries.Infrastructure.Repositories
{
    public class CachedEntryRepository : EntryRepository, IEntryRepository
    {
        private EntryCacheIO _entryCacheIO;

        public CachedEntryRepository(EntryContext context,
                                     EntryCacheIO entryCacheIO)
            : base(context)
        {
            _entryCacheIO = entryCacheIO;
        }

        public new async Task<Result<IEnumerable<Entry>>> Get(Guid userId)
        {
            Result<IEnumerable<Entry>> cachedEntriesResult = await _entryCacheIO.Get(userId);

            if (cachedEntriesResult.Success)
                return cachedEntriesResult;

            Result<IEnumerable<Entry>> entriesResult = await base.Get(userId);

            await _entryCacheIO.Set(userId, entriesResult);

            return entriesResult;
        }

        public new async Task<Result<Entry>> Get(Guid userId, Guid entryId)
        {
            Result<Entry> cachedEntryResult = await _entryCacheIO.Get(userId, 
                                                                      entry => entry.Id == entryId,
                                                                      $"The entry ({entryId}) was not found.");

            if(cachedEntryResult.Success)
                return cachedEntryResult;

            Result<Entry> entryResult = await base.Get(userId, entryId);

            return entryResult;
        }

        public new async Task<Result> Create(Guid userId, Entry entry)
        {
            Result creationResult = await base.Create(userId, entry);

            if (creationResult.Success)
                await _entryCacheIO.Remove(userId);

            return creationResult;
        }

        public new async Task<Result> Update(Guid userId, Entry entry)
        {
            Result updateResult = await base.Update(userId, entry);

            if (updateResult.Success)
                await _entryCacheIO.Remove(userId);

            return updateResult;
        }

        public new async Task<Result> Delete(Guid userId, Guid entryId)
        {
            Result deleteResult = await base.Delete(userId, entryId);

            if(deleteResult.Success)
                await _entryCacheIO.Remove(userId);

            return deleteResult;
        }

        public new async Task<Result> DeleteByCategory(Guid userId, Guid categoryId)
        {
            Result deleteResult = await base.DeleteByCategory(userId, categoryId);

            if (deleteResult.Success)
                await _entryCacheIO.Remove(userId);

            return deleteResult;
        }
    }
}
