namespace ToDo.Microservices.Identity.Database.Entities
{
    public class PermissionEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<RoleEntity> Roles { get; set; }
    }
}
