namespace ToDo.Microservices.Entries.API.Contracts.Entries
{
    public class EntriesContractCreate
    {
        public EntriesContractCreate(Guid categoryId,
                                     string text,
                                     DateTime? deadline)
        {
            CategoryId = categoryId;
            Text = text;
            Deadline = deadline;
        }

        public Guid CategoryId { get; private set; }

        public string Text { get; private set; }

        public DateTime? Deadline { get; private set; }
    }
}
