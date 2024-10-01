namespace ToDo.Microservices.Categories.Database.Entities
{
    public class CategoryEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }


        public Guid UserId { get; set; }

        public UserEntity User { get; set; }
    }
}
