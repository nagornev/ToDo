using ToDo.Microservices.Entries.Database.Entities;
using ToDo.Microservices.Entries.Domain.Models;

namespace ToDo.Microservices.Entries.Database.Extensions
{
    public static class EntryEntityExtensions
    {
        public static Entry GetDomain(this EntryEntity entryEntity)
        {
            return Entry.Constructor(entryEntity.Id,
                                     entryEntity.CategoryId,
                                     entryEntity.Text,
                                     entryEntity.Deadline,
                                     entryEntity.Completed);
        }

        public static IEnumerable<Entry> GetDomain(this IEnumerable<EntryEntity> entryEntities)
        {
            return entryEntities.Select(x => x.GetDomain());
        }
    }
}
