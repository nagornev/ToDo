using ToDo.Domain.Results;
using ToDo.Microservices.Entries.Domain.Models;
using ToDo.Microservices.Entries.Infrastructure.Cachers;
using ToDo.Microservices.Entries.UseCases.Cachers;
using ToDo.Microservices.Entries.UseCases.Repositories;

namespace ToDo.Microservices.Entries.Infrastructure.Repositories
{
    public class CachedEntryRepository : IEntryRepository
    {
        private EntryRepository _entryRespository;
                
        private IEntryCacher _entryCacher;

        public CachedEntryRepository(EntryRepository entryRespository,
                                     IEntryCacher entryCacher)
        {
            _entryRespository = entryRespository;
            _entryCacher = entryCacher;
        }

        public async Task<Result<IEnumerable<Entry>>> Get(Guid userId)
        {
            Result<IEnumerable<Entry>> cachedEntriesResult = await _entryCacher.Get(userId);

            if (cachedEntriesResult.Success)
                return cachedEntriesResult;

            Result<IEnumerable<Entry>> entriesResult = await _entryRespository.Get(userId);

            await _entryCacher.Set(userId, entriesResult);

            return entriesResult;
        }

        public async Task<Result<Entry>> Get(Guid userId, Guid entryId)
        {
            Result<Entry> cachedEntryResult = await _entryCacher.Get(userId, entryId);

            if(cachedEntryResult.Success)
                return cachedEntryResult;

            Result<Entry> entryResult = await _entryRespository.Get(userId, entryId);

            return entryResult;
        }

        public async Task<Result> Create(Guid userId, Entry entry)
        {
            Result creationResult = await _entryRespository.Create(userId, entry);

            if (creationResult.Success)
                await _entryCacher.Remove(userId);

            return creationResult;
        }

        public async Task<Result> Update(Guid userId, Entry entry)
        {
            Result updateResult = await _entryRespository.Update(userId, entry);

            if (updateResult.Success)
                await _entryCacher.Remove(userId);

            return updateResult;
        }

        public async Task<Result> Delete(Guid userId, Guid entryId)
        {
            Result deleteResult = await _entryRespository.Delete(userId, entryId);

            if(deleteResult.Success)
                await _entryCacher.Remove(userId);

            return deleteResult;
        }

        public async Task<Result> DeleteByCategory(Guid userId, Guid categoryId)
        {
            Result deleteResult = await _entryRespository.DeleteByCategory(userId, categoryId);

            if (deleteResult.Success)
                await _entryCacher.Remove(userId);

            return deleteResult;
        }
    }
}
