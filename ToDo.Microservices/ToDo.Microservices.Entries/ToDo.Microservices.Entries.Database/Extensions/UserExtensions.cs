using ToDo.Microservices.Entries.Database.Entities;
using ToDo.Microservices.Entries.Domain.Models;

namespace ToDo.Microservices.Entries.Database.Extensions
{
    public static class UserExtensions
    {
        public static UserEntity GetEntity(this User user, IEnumerable<Entry> entries = null)
        {
            return new UserEntity()
            {
                Id = user.Id,
                Entries = entries?.GetEntity(user.Id),
            };
        }
    }
}
