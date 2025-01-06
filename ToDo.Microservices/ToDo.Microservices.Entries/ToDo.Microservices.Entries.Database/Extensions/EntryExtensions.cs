using ToDo.Microservices.Entries.Database.Entities;
using ToDo.Microservices.Entries.Domain.Models;

namespace ToDo.Microservices.Entries.Database.Extensions
{
    public static class EntryExtensions
    {
        public static EntryEntity GetEntity(this Entry entry, Guid userId)
        {
            return new EntryEntity()
            {
                Id = entry.Id,
                CategoryId = entry.CategoryId,

                UserId = userId,
            };
        }

        public static EntryEntity GetEntity(this Entry entry, UserEntity userEntity)
        {
            return new EntryEntity()
            {
                Id = entry.Id,
                CategoryId = entry.CategoryId,

                UserId = userEntity.Id,
                User = userEntity,
            };
        }

        public static IEnumerable<EntryEntity> GetEntity(this IEnumerable<Entry> entry, Guid userId)
        {
            return entry.Select(x => x.GetEntity(userId));
        }

        public static IEnumerable<EntryEntity> GetEntity(this IEnumerable<Entry> entry, UserEntity userEntity)
        {
            return entry.Select(x => x.GetEntity(userEntity));
        }
    }
}
