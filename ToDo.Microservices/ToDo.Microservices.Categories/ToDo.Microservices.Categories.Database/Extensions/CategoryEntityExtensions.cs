using System.Runtime.CompilerServices;
using ToDo.Domain.Results;
using ToDo.Microservices.Categories.Database.Entities;
using ToDo.Microservices.Categories.Domain.Models;

namespace ToDo.Microservices.Categories.Database.Extensions
{
    public static class CategoryEntityExtensions
    {
        public static Result<Category> GetDomain(this CategoryEntity categoryEntity)
        {
            return Category.Constructor(categoryEntity.Id, categoryEntity.Name);
        }

        public static Result<IEnumerable<Category>> GetDomain(this IEnumerable<CategoryEntity> categoryEntities)
        {
            List<Category> categories = new List<Category>();

            foreach(CategoryEntity categoryEntity in categoryEntities)
            {
                Result<Category> categoryResult = categoryEntity.GetDomain();

                if (!categoryResult.Success)
                    return Result<IEnumerable<Category>>.Failure(categoryResult.Error);

                categories.Add(categoryResult.Content);
            }

            return Result<IEnumerable<Category>>.Successful(categories);
        }
    }
}
