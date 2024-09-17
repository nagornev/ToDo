namespace ToDo.Microservices.Entries.API.Contracts.Entries
{
    public class EntriesContractDelete
    {
        public EntriesContractDelete(Guid entryId)
        {
            EntryId = entryId;
        }

        public Guid EntryId { get; private set; }
    }
}
