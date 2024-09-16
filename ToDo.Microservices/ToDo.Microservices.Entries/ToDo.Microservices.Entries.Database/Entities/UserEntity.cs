namespace ToDo.Microservices.Entries.Database.Entities
{
    public class UserEntity
    {
        public Guid Id { get; set; }

        public IEnumerable<EntryEntity> Entries { get; set; }
    }
}
