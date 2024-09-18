using Microsoft.EntityFrameworkCore;
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


        public async Task<IEnumerable<Entry>> Get(Guid userId)
        {
            IEnumerable<EntryEntity> entryEntities = await _entryContext.Entries.AsNoTracking()
                                                                                .Where(x=>x.UserId == userId)
                                                                                .ToListAsync();

            IEnumerable<Entry> entries = entryEntities.Select(x => Entry.Constructor(x.Id, x.CategoryId, x.Text, x.Deadline, x.Completed));

            return entries;
        }

        public async Task<Entry?> Get(Guid userId, Guid entryId)
        {
            EntryEntity? entryEntity = await _entryContext.Entries.AsNoTracking()
                                                                  .FirstOrDefaultAsync(x => x.UserId == userId &&
                                                                                            x.Id == entryId);

            return entryEntity is not null ?
                        Entry.Constructor(entryEntity.Id,
                                          entryEntity.CategoryId,
                                          entryEntity.Text,
                                          entryEntity.Deadline,
                                          entryEntity.Completed) :
                        default; ;
        }

        public async Task<bool> Create(Guid userId, Entry entry)
        {
            EntryEntity entryEntity = new EntryEntity()
            {
                Id = entry.Id,
                CategoryId = entry.CategoryId,
                Text = entry.Text,
                Deadline = entry.Deadline,
                Completed = entry.Completed,

                UserId = userId,
            };

            await _entryContext.Entries.AddAsync(entryEntity);

            try
            {
                int rows = await _entryContext.SaveChangesAsync();
                return rows > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Update(Guid userId, Entry entry)
        {
            int rows = await _entryContext.Entries.Where(x => x.UserId == userId &&
                                                              x.Id == entry.Id)
                                                  .ExecuteUpdateAsync(x => x.SetProperty(p => p.CategoryId, entry.CategoryId)
                                                                            .SetProperty(p => p.Text, entry.Text)
                                                                            .SetProperty(p => p.Deadline, entry.Deadline)
                                                                            .SetProperty(p => p.Completed, entry.Completed));

            return rows > 0;
        }

        public async Task<bool> Delete(Guid userId, Guid entryId)
        {
            int rows = await _entryContext.Entries.Where(x => x.UserId == userId &&
                                                            x.Id == entryId)
                                                  .ExecuteDeleteAsync();

            return rows > 0;
        }

    }
}
