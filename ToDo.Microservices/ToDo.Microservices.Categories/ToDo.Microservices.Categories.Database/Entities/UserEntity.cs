namespace ToDo.Microservices.Categories.Database.Entities
{
    public class UserEntity
    {
        public Guid Id { get; set; }

        public IEnumerable<CategoryEntity> Categories { get; set; }
    }
}
