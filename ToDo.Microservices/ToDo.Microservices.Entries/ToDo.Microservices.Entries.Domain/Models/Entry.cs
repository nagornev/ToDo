using System;
using System.Text.Json.Serialization;

namespace ToDo.Microservices.Entries.Domain.Models
{
    public class Entry
    {
        public const int TextMaximumLength = 100;

        [JsonConstructor]
        private Entry(Guid id,
                      Guid categoryId,
                      string text,
                      DateTime? deadline,
                      bool completed)
        {
            Id = id;
            CategoryId = categoryId;
            Text = text;
            Deadline = deadline;
            Completed = completed;
        }

        public Guid Id { get; private set; }

        public Guid CategoryId { get; private set; }

        public string Text { get; private set; }

        public DateTime? Deadline { get; private set; }

        public bool Completed { get; private set; }

        public static Entry Constructor(Guid id,
                                        Guid categoryId,
                                        string text,
                                        DateTime? deadline,
                                        bool completed)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException("The entry id can not be empty.");

            if (categoryId == Guid.Empty)
                throw new ArgumentNullException("The category id can not be empty.");

            return new Entry(id, categoryId, text, deadline, completed);
        }

        public static Entry New(Guid categoryId, string text, DateTime? deadline)
        {
            return Constructor(Guid.NewGuid(), categoryId, text, deadline, false);
        }
    }
}
