using Microsoft.EntityFrameworkCore;
using ToDo.Microservices.Identity.Database.Contexts;
using ToDo.Microservices.Identity.Database.Extensions;
using ToDo.Microservices.Identity.Domain.Models;

namespace ToDo.Microservices.Identity.Tests.Mock
{
    internal class IdentityContextMock : IdentityContext
    {
        public IReadOnlyCollection<User> Data { get; }

        public IdentityContextMock(string dbName, Func<IReadOnlyCollection<User>> data = null)
           : base(new DbContextOptionsBuilder<IdentityContext>()
                           .UseSqlite($"DataSource=file:{dbName}?mode=memory&cache=shared")
                           .Options)
        {
            Database.OpenConnection();
            Database.EnsureCreated();

            Data = data?.Invoke() ?? Array.Empty<User>();

            Initialize();
        }

        private void Initialize()
        {
            foreach (User user in Data)
            {
                var userEntity = user.GetEntity();

                Users.Add(userEntity);

                SaveChanges();
            }
        }
    }
}
