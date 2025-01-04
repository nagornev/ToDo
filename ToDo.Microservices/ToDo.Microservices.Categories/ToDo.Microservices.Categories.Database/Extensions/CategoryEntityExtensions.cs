using System.Runtime.CompilerServices;
using ToDo.Microservices.Categories.Database.Entities;
using ToDo.Microservices.Categories.Domain.Models;

namespace ToDo.Microservices.Categories.Database.Extensions
{
    public static class CategoryEntityExtensions
    {
        public static Category GetDomain(this CategoryEntity categoryEntity)
        {
            return Category.Constructor(categoryEntity.Id, categoryEntity.Name);
        }

        public static IEnumerable<Category> GetDomain(this IEnumerable<CategoryEntity> categoryEntities)
        {
            return categoryEntities.Select(x => x.GetDomain());
        }
    }
}
