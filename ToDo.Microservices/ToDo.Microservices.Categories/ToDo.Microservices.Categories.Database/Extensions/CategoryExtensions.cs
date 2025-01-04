using ToDo.Microservices.Categories.Database.Entities;
using ToDo.Microservices.Categories.Domain.Models;

namespace ToDo.Microservices.Categories.Database.Extensions
{
    public static class CategoryExtensions
    {
        public static CategoryEntity GetEntity(this Category category, Guid userId)
        {
            return new CategoryEntity
            {
                Id = category.Id,
                Name = category.Name,

                UserId = userId,
            };
        }

        public static CategoryEntity GetEntity(this Category category, UserEntity userEntity)
        {
            return new CategoryEntity
            {
                Id = category.Id,
                Name = category.Name,

                UserId = userEntity.Id,
                User = userEntity,
            };
        }

        public static IEnumerable<CategoryEntity> GetEntity(this IEnumerable<Category> categories, Guid userId)
        {
            return categories.Select(x => x.GetEntity(userId));
        }

        public static IEnumerable<CategoryEntity> GetEntity(this IEnumerable<Category> categories, UserEntity userEntity)
        {
            return categories.Select(x => x.GetEntity(userEntity));
        }
    }
}
