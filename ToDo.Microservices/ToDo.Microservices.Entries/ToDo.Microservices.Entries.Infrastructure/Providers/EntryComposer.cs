using ToDo.Microservices.Entries.Domain.Collectings;
using ToDo.Microservices.Entries.Domain.Models;
using ToDo.Microservices.Entries.UseCases.Providers;

namespace ToDo.Microservices.Entries.Infrastructure.Providers
{
    public class EntryComposer : IEntryСomposer
    {
        private const string _undefined = "Undefined";

        public IEnumerable<EntryCompose> Compose(IEnumerable<Entry> entries, IEnumerable<Category> categories)
        {
            List<EntryCompose> composes = new List<EntryCompose>();

            foreach (Entry entry in entries)
            {
                Category? category = categories.FirstOrDefault(x => x.Id == entry.CategoryId);

                composes.Add(new EntryCompose(entry, category ?? GetUndefinedCategory(entry)));
            }

            return composes;
        }

        public EntryCompose Compose(Entry entry, Category category)
        {
            return new EntryCompose(entry, category ?? GetUndefinedCategory(entry));
        }

        private Category GetUndefinedCategory(Entry entry)
        {
            return new Category(entry.CategoryId, _undefined);
        }
    }
}
