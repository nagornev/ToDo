using Microsoft.EntityFrameworkCore;
using ToDo.Microservices.Categories.Database.Contexts;
using ToDo.Microservices.Categories.Database.Entities;
using ToDo.Microservices.Categories.Domain.Models;

namespace ToDo.Microservices.Categories.Tests.Mock
{
    public class CategoryContextMock : CategoryContext
    {
        public IReadOnlyDictionary<User, IEnumerable<Category>> Data { get; }

        public IEnumerable<User> DefaultUsers => Data.Keys;

        public IEnumerable<IEnumerable<Category>> DefaultCategories => Data.Values;

        public CategoryContextMock(string dbName, Func<IReadOnlyDictionary<User, IEnumerable<Category>>> data = null)
            : base(new DbContextOptionsBuilder<CategoryContext>()
                            .UseInMemoryDatabase(dbName)
                            .Options)
        {
            Data = data?.Invoke() ?? new Dictionary<User, IEnumerable<Category>>();

            Initialize();
        }

        private void Initialize()
        {

            foreach (User user in Data.Keys)
            {
                UserEntity userEntity = new UserEntity()
                {
                    Id = user.Id,
                };

                Users.Add(userEntity);

                foreach (Category category in Data[user])
                {
                    CategoryEntity categoryEntity = new CategoryEntity()
                    {
                        Id = category.Id,
                        Name = category.Name,

                        UserId = user.Id,
                    };

                    Categories.Add(categoryEntity);
                }

                SaveChanges();
            }
        }
    }
}
