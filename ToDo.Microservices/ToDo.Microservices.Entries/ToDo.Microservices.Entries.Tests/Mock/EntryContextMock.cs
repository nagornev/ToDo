using Microsoft.EntityFrameworkCore;
using ToDo.Microservices.Entries.Database.Contexts;
using ToDo.Microservices.Entries.Database.Entities;
using ToDo.Microservices.Entries.Database.Extensions;
using ToDo.Microservices.Entries.Domain.Models;

namespace ToDo.Microservices.Entries.Tests.Mock
{
    public class EntryContextMock : EntryContext
    {
        public IReadOnlyDictionary<User, IEnumerable<Entry>> Data { get; }

        public EntryContextMock(string dbName, Func<IReadOnlyDictionary<User, IEnumerable<Entry>>> data = null)
            : base(new DbContextOptionsBuilder<EntryContext>()
                            .UseSqlite($"DataSource=file:{dbName}?mode=memory&cache=shared")
                            .Options)
        {
            Database.OpenConnection();
            Database.EnsureCreated();

            Data = data?.Invoke() ?? new Dictionary<User, IEnumerable<Entry>>();

            Initialize();
        }

        private void Initialize()
        {

            foreach (User user in Data.Keys)
            {
                UserEntity userEntity = user.GetEntity();

                Users.Add(userEntity);

                foreach (Entry entry in Data[user])
                {
                    EntryEntity entryEntity = entry.GetEntity(user.Id);

                    Entries.Add(entryEntity);
                }

                SaveChanges();
            }
        }
    }
}
