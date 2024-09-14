using ToDo.Microservices.Entries.Domain.Models;

namespace ToDo.Microservices.Entries.Domain.Collectings
{
    public class EntryCompose
    {
        public EntryCompose(Entry entry, Category category)
        {
            Entry = entry;
            Category = category;
        }

        public Entry Entry { get; private set; }

        public Category Category { get; private set; }
    }
}
