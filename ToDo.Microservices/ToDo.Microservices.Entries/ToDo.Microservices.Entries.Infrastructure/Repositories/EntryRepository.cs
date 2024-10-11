using Microsoft.EntityFrameworkCore;
using ToDo.Domain.Results;
using ToDo.Microservices.Entries.Database.Contexts;
using ToDo.Microservices.Entries.Database.Entities;
using ToDo.Microservices.Entries.Domain.Models;
using ToDo.Microservices.Entries.UseCases.Repositories;

namespace ToDo.Microservices.Entries.Infrastructure.Repositories
{
    public class EntryRepository : IEntryRepository
    {
        private EntryContext _entryContext;

        public EntryRepository(EntryContext entryContext)
        {
            _entryContext = entryContext;
        }

        public async Task<Result<IEnumerable<Entry>>> Get(Guid userId)
        {
            IEnumerable<EntryEntity> entryEntities = await _entryContext.Entries.AsNoTracking()
                                                                                .Where(x => x.UserId == userId)
                                                                                .ToListAsync();

            IEnumerable<Entry> entries = entryEntities.Select(x => Entry.Constructor(x.Id, x.CategoryId, x.Text, x.Deadline, x.Completed));

            return Result<IEnumerable<Entry>>.Successful(entries);
        }

        public async Task<Result<Entry>> Get(Guid userId, Guid entryId)
        {
            EntryEntity? entryEntity = await _entryContext.Entries.AsNoTracking()
                                                                  .FirstOrDefaultAsync(x => x.UserId == userId &&
                                                                                            x.Id == entryId);

            return entryEntity is not null ?
                        Result<Entry>.Successful(Entry.Constructor(entryEntity.Id,
                                                                   entryEntity.CategoryId,
                                                                   entryEntity.Text,
                                                                   entryEntity.Deadline,
                                                                   entryEntity.Completed)) :
                        Result<Entry>.Failure(Errors.IsNull($"The entry {entryId} was not found"));
        }

        public async Task<Result> Create(Guid userId, Entry entry)
        {
            EntryEntity entryEntity = CreateEntryEntity(userId, entry);

            await _entryContext.Entries.AddAsync(entryEntity);

            return await _entryContext.SaveChangesAsync() > 0 ?
                      Result.Successful() :
                      Result.Failure(Errors.IsMessage("The entry was not created. Please check entry parameters and try again later."));
        }

        public async Task<Result> Update(Guid userId, Entry entry)
        {
            return await _entryContext.Entries.Where(x => x.UserId == userId &&
                                                              x.Id == entry.Id)
                                                  .ExecuteUpdateAsync(x => x.SetProperty(p => p.CategoryId, entry.CategoryId)
                                                                            .SetProperty(p => p.Text, entry.Text)
                                                                            .SetProperty(p => p.Deadline, entry.Deadline)
                                                                            .SetProperty(p => p.Completed, entry.Completed)) > 0 ?
                        Result.Successful() :
                        Result.Failure(Errors.IsNull($"The entry {entry.Id} was not found."));
        }

        public async Task<Result> Delete(Guid userId, Guid entryId)
        {
            return await _entryContext.Entries.Where(x => x.UserId == userId &&
                                                            x.Id == entryId)
                                                  .ExecuteDeleteAsync() > 0 ?
                        Result.Successful() :
                        Result.Failure(Errors.IsNull($"The entry {entryId} was not found."));
        }

        public async Task<Result> DeleteByCategory(Guid userId, Guid categoryId)
        {
            return await _entryContext.Entries.Where(x => x.UserId == userId &&
                                                          x.CategoryId == categoryId)
                                              .ExecuteDeleteAsync() > 0 ?
                         Result.Successful() :
                         Result.Failure(Errors.IsNull($"The entry with category {categoryId} was not found."));
        }

        private EntryEntity CreateEntryEntity(Guid userId, Entry entry)
        {
            return new EntryEntity()
            {
                Id = entry.Id,
                CategoryId = entry.CategoryId,
                Text = entry.Text,
                Deadline = entry.Deadline,
                Completed = entry.Completed,

                UserId = userId,
            };
        }
    }
}
