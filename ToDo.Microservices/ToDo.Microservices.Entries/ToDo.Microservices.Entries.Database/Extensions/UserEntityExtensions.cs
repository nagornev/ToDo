using ToDo.Microservices.Entries.Database.Entities;
using ToDo.Microservices.Entries.Domain.Models;

namespace ToDo.Microservices.Entries.Database.Extensions
{
    public static class UserEntityExtensions
    {
        public static User GetDomain(this UserEntity userEntity)
        {
            return User.Constructor(userEntity.Id);
        }
    }
}
