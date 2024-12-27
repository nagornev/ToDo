using ToDo.Domain.Results;
using ToDo.Microservices.Entries.Domain.Models;
using ToDo.Microservices.Entries.Infrastructure.Cachers;
using ToDo.Microservices.Entries.UseCases.Repositories;
using ToDo.Cache.Abstractions.Extensions;

namespace ToDo.Microservices.Entries.Infrastructure.Repositories
{
    public class CachedEntryRepository : IEntryRepository
    {
        private EntryRepository _entryRepository;

        private EntryCacheIO _entryCacheIO;

        public CachedEntryRepository(EntryRepository entryRepository,
                                     EntryCacheIO entryCacheIO)
        {
            _entryRepository = entryRepository;
            _entryCacheIO = entryCacheIO;
        }

        public async Task<Result<IEnumerable<Entry>>> Get(Guid userId)
        {
            Result<IEnumerable<Entry>> cachedEntriesResult = await _entryCacheIO.Get(userId);

            if (cachedEntriesResult.Success)
                return cachedEntriesResult;

            Result<IEnumerable<Entry>> entriesResult = await _entryRepository.Get(userId);

            await _entryCacheIO.Set(userId, entriesResult);

            return entriesResult;
        }

        public async Task<Result<Entry>> Get(Guid userId, Guid entryId)
        {
            Result<Entry> cachedEntryResult = await _entryCacheIO.Get(userId, 
                                                                      entry => entry.Id == entryId,
                                                                      $"The entry ({entryId}) was not found.");

            if(cachedEntryResult.Success)
                return cachedEntryResult;

            Result<Entry> entryResult = await _entryRepository.Get(userId, entryId);

            return entryResult;
        }

        public async Task<Result> Create(Guid userId, Entry entry)
        {
            Result creationResult = await _entryRepository.Create(userId, entry);

            if (creationResult.Success)
                await _entryCacheIO.Remove(userId);

            return creationResult;
        }

        public async Task<Result> Update(Guid userId, Entry entry)
        {
            Result updateResult = await _entryRepository.Update(userId, entry);

            if (updateResult.Success)
                await _entryCacheIO.Remove(userId);

            return updateResult;
        }

        public async Task<Result> Delete(Guid userId, Guid entryId)
        {
            Result deleteResult = await _entryRepository.Delete(userId, entryId);

            if(deleteResult.Success)
                await _entryCacheIO.Remove(userId);

            return deleteResult;
        }

        public async Task<Result> DeleteByCategory(Guid userId, Guid categoryId)
        {
            Result deleteResult = await _entryRepository.DeleteByCategory(userId, categoryId);

            if (deleteResult.Success)
                await _entryCacheIO.Remove(userId);

            return deleteResult;
        }
    }
}
