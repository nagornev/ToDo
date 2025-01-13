namespace ToDo.Microservices.Entries.API.Contracts.Entries
{
    public class EntriesContractComplete
    {
        public EntriesContractComplete(Guid entryId,
                                       bool completed)
        {
            EntryId = entryId;
            Completed = completed;
        }

        public Guid EntryId { get; private set; }

        public bool Completed { get; private set; }
    }
}
