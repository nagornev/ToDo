using ToDo.Microservices.Entries.Domain.Collectings;
using ToDo.Microservices.Entries.Domain.Models;
using ToDo.Microservices.Entries.UseCases.Providers;

namespace ToDo.Microservices.Entries.Infrastructure.Providers
{
    public class EntryComposer : IEntryСomposer
    {
        public IEnumerable<EntryCompose> Compose(IEnumerable<Entry> entries, IEnumerable<Category> categories)
        {
            List<EntryCompose> composes = new List<EntryCompose>();

            foreach (Entry entry in entries)
            {
                Category? category = categories.FirstOrDefault(x => x.Id == entry.CategoryId);

                //Пропуск записи нужен для того, чтобы не отображать записи с удаленными категориями, которые ещё не были обработаны
                //месседж брокером: каскадно удалить все записи связанные с удаленной категорией.
                if (category is null)
                    continue;

                composes.Add(new EntryCompose(entry, category));
            }

            return composes;
        }

        public EntryCompose Compose(Entry entry, Category category)
        {
            return new EntryCompose(entry, category);
        }
    }
}
