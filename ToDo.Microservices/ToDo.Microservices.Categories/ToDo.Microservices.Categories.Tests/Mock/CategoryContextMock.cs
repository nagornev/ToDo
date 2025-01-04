using Microsoft.EntityFrameworkCore;
using ToDo.Microservices.Categories.Database.Contexts;
using ToDo.Microservices.Categories.Database.Entities;
using ToDo.Microservices.Categories.Database.Extensions;
using ToDo.Microservices.Categories.Domain.Models;

namespace ToDo.Microservices.Categories.Tests.Mock
{
    public class CategoryContextMock : CategoryContext
    {
        public IReadOnlyDictionary<User, IEnumerable<Category>> Data { get; }

        public CategoryContextMock(string dbName, Func<IReadOnlyDictionary<User, IEnumerable<Category>>> data = null)
            : base(new DbContextOptionsBuilder<CategoryContext>()
                            .UseSqlite($"DataSource=file:{dbName}?mode=memory&cache=shared")
                            .Options)
        {
            Database.OpenConnection();
            Database.EnsureCreated();

            Data = data?.Invoke() ?? new Dictionary<User, IEnumerable<Category>>();

            Initialize();
        }

        private void Initialize()
        {

            foreach (User user in Data.Keys)
            {
                UserEntity userEntity = user.GetEntity();

                Users.Add(userEntity);

                foreach (Category category in Data[user])
                {
                    CategoryEntity categoryEntity = category.GetEntity(user.Id);

                    Categories.Add(categoryEntity);
                }

                SaveChanges();
            }
        }
    }
}
