using System;
using System.Text.Json.Serialization;
using ToDo.Domain.Results;

namespace ToDo.Microservices.Entries.Domain.Models
{
    [Serializable]
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

        public static Result<Entry> Constructor(Guid id,
                                                Guid categoryId,
                                                string text,
                                                DateTime? deadline,
                                                bool completed)
        {
            if (id == Guid.Empty)
                return Result<Entry>.Failure(error => error.NullOrEmpty("The entry ID can`t be null or empty.", nameof(Id)));

            if (categoryId == Guid.Empty)
                return Result<Entry>.Failure(error => error.NullOrEmpty("The category ID can`t be null or empty.", nameof(CategoryId)));

            if (string.IsNullOrEmpty(text) ||
                string.IsNullOrWhiteSpace(text) ||
                text.Length > TextMaximumLength)
                return Result<Entry>.Failure(error => error.InvalidArgument($"The entry text can`t be null or empty and more than {TextMaximumLength} symbols.", nameof(Text)));

            return Result<Entry>.Successful(new Entry(id, categoryId, text, deadline, completed));
        }

        public static Result<Entry> New(Guid categoryId, string text, DateTime? deadline)
        {
            return Constructor(Guid.NewGuid(), categoryId, text, deadline, false);
        }
    }
}
