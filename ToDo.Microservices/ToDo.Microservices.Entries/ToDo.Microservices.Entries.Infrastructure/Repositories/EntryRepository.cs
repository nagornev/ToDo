using Microsoft.EntityFrameworkCore;
using ToDo.Domain.Results;
using ToDo.Domain.Results.Extensions;
using ToDo.Microservices.Entries.Database.Contexts;
using ToDo.Microservices.Entries.Database.Entities;
using ToDo.Microservices.Entries.Database.Extensions;
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
            UserEntity? userEntity = await _entryContext.Users.AsNoTracking()
                                                              .Include(x => x.Entries)
                                                              .FirstOrDefaultAsync(x => x.Id == userId);

            return userEntity is not null ?
                        Result<IEnumerable<Entry>>.Successful(userEntity.Entries.GetDomain()) :
                        Result<IEnumerable<Entry>>.Failure(error => error.NullOrEmpty($"The user {userId} was not found."));
        }

        public async Task<Result<Entry>> Get(Guid userId, Guid entryId)
        {
            EntryEntity? entryEntity = await _entryContext.Entries.AsNoTracking()
                                                                  .FirstOrDefaultAsync(x => x.UserId == userId &&
                                                                                            x.Id == entryId);

            return entryEntity is not null ?
                        Result<Entry>.Successful(entryEntity.GetDomain()) :
                        Result<Entry>.Failure(error => error.NullOrEmpty($"The entry {entryId} was not found. Please check get entry parameters and try again later."));
        }

        public async Task<Result> Create(Guid userId, Entry entry)
        {
            await _entryContext.Entries.AddAsync(entry.GetEntity(userId));

            return await _entryContext.SaveChangesAsync() > 0 ?
                      Result.Successful() :
                      Result.Failure(error => error.NullOrEmpty("The entry was not created. Please check entry create parameters and try again later."));
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
                        Result.Failure(error => error.NullOrEmpty($"The entry {entry.Id} was not found. Please check entry update parameters and try again later."));
        }

        public async Task<Result> Delete(Guid userId, Guid entryId)
        {
            return await _entryContext.Entries.Where(x => x.UserId == userId &&
                                                            x.Id == entryId)
                                                  .ExecuteDeleteAsync() > 0 ?
                        Result.Successful() :
                        Result.Failure(error => error.NullOrEmpty($"The entry {entryId} was not found. Please check entry delete parameters and try again later."));
        }

        public async Task<Result> DeleteByCategory(Guid userId, Guid categoryId)
        {
            return await _entryContext.Entries.Where(x => x.UserId == userId &&
                                                          x.CategoryId == categoryId)
                                              .ExecuteDeleteAsync() > 0 ?
                         Result.Successful() :
                         Result.Failure(error => error.NullOrEmpty($"The entry with category {categoryId} was not found. Please check entry delete parameters and try again later."));
        }
    }
}
