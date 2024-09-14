using System.Collections.Generic;
using ToDo.Microservices.Entries.Domain.Collectings;
using ToDo.Microservices.Entries.Domain.Models;

namespace ToDo.Microservices.Entries.UseCases.Providers
{
    public interface IEntryСomposer
    {
        IEnumerable<EntryCompose> Сompose(IEnumerable<Entry> entries, IEnumerable<Category> categories);

        EntryCompose Сompose(Entry entry, Category category);
    }
}
