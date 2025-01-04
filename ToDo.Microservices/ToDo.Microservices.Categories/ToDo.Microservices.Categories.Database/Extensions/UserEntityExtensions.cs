using ToDo.Microservices.Categories.Database.Entities;
using ToDo.Microservices.Categories.Domain.Models;

namespace ToDo.Microservices.Categories.Database.Extensions
{
    public static class UserEntityExtensions
    {
        public static User GetDomain(this UserEntity userEntity)
        {
            return User.Constructor(userEntity.Id);
        }
    }
}
