using System;
using ToDo.Microservices.Entries.Domain.Models;

namespace ToDo.Microservices.Entries.Domain.Collectings
{
    public class EntryCompose
    {
        public EntryCompose(Entry entry, Category category)
        {
            Id = entry.Id;
            Category = category;
            Text = entry.Text;
            Deadline = entry.Deadline;
            Completed = entry.Completed;
        }

        public Guid Id { get; private set; }

        public Category Category { get; private set; }

        public string Text { get; private set; }

        public DateTime? Deadline { get; private set; }

        public bool Completed { get; private set; }
    }
}
