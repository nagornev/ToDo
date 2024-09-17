namespace ToDo.Microservices.Entries.API.Contracts.Entries
{
    public class EntriesContractUpdate
    {
        public EntriesContractUpdate(Guid entryId,
                                     Guid categoryId,
                                     string text,
                                     DateTime? deadline,
                                     bool completed = false)
        {
            EntryId = entryId;
            CategoryId = categoryId;
            Text = text;
            Deadline = deadline;
            Completed = completed;
        }

        public Guid EntryId { get; private set; }

        public Guid CategoryId { get; private set; }

        public string Text { get; private set; }

        public DateTime? Deadline { get; private set; }

        public bool Completed { get; private set; }
    }
}
