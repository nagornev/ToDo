using ToDo.Domain.Results;
using ToDo.Microservices.Entries.Database.Entities;
using ToDo.Microservices.Entries.Domain.Models;

namespace ToDo.Microservices.Entries.Database.Extensions
{
    public static class EntryEntityExtensions
    {
        public static Result<Entry> GetDomain(this EntryEntity entryEntity)
        {
            return Entry.Constructor(entryEntity.Id,
                                     entryEntity.CategoryId,
                                     entryEntity.Text,
                                     entryEntity.Deadline,
                                     entryEntity.Completed);
        }

        public static Result<IEnumerable<Entry>> GetDomain(this IEnumerable<EntryEntity> entryEntities)
        {
            List<Entry> entries = new List<Entry>();

            foreach (EntryEntity entryEntity in entryEntities)
            {
                Result<Entry> entryResult = entryEntity.GetDomain();

                if (!entryResult.Success)
                    return Result<IEnumerable<Entry>>.Failure(entryResult.Error);

                entries.Add(entryResult.Content);
            }


            return Result<IEnumerable<Entry>>.Successful(entries);
        }
    }
}
