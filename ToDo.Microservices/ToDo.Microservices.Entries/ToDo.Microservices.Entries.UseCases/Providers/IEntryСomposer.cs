using System.Collections.Generic;
using ToDo.Microservices.Entries.Domain.Collectings;
using ToDo.Microservices.Entries.Domain.Models;

namespace ToDo.Microservices.Entries.UseCases.Providers
{
    /// <summary>
    /// Collects entries and categories in one class.
    /// </summary>
    public interface IEntryСomposer
    {
        IEnumerable<EntryCompose> Compose(IEnumerable<Entry> entries, IEnumerable<Category> categories);

        EntryCompose Compose(Entry entry, Category category);
    }
}
