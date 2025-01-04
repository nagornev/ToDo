using ToDo.Microservices.Categories.Database.Entities;
using ToDo.Microservices.Categories.Domain.Models;

namespace ToDo.Microservices.Categories.Database.Extensions
{
    public static class UserExtensions
    {
        public static UserEntity GetEntity(this User user, IEnumerable<Category> categories = null)
        {
            return new UserEntity
            {
                Id = user.Id,
                Categories = categories?.GetEntity(user.Id),
            };
        }
    }
}
