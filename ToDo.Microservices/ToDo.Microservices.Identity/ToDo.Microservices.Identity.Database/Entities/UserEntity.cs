namespace ToDo.Microservices.Identity.Database.Entities
{
    public class UserEntity
    {

        public Guid Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }


        public int RoleId { get; set; }

        public RoleEntity Role { get; set; }
    }
}
