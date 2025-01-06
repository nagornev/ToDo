using ToDo.Microservices.Entries.Domain.Models;
using ToDo.Microservices.Entries.Tests.Mock;

namespace ToDo.Microservices.Entries.Tests.Units
{
    public class EntryRepositoryTests
    {
        private EntryContextMock GetEntryContextMock(Func<IReadOnlyDictionary<User, IEnumerable<Entry>>> startContext = null)
        {
            return new EntryContextMock($"{Guid.NewGuid()}", startContext);
        }
    }
}
