using Microsoft.EntityFrameworkCore;
using ToDo.Microservices.Identity.Database.Configurations;
using ToDo.Microservices.Identity.Database.Entities;

namespace ToDo.Microservices.Identity.Database.Contexts
{
    public class IdentityContext : DbContext
    {
        public IdentityContext(DbContextOptions<IdentityContext> options)
            : base(options)
        {
        }

        public DbSet<UserEntity> Users { get; private set; }

        public DbSet<RoleEntity> Roles { get; private set; }

        public DbSet<PermissionEntity> Permissions { get; private set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new PermissionConfiguration());
            modelBuilder.ApplyConfiguration(new RolePermissionConfiguration());
        }
    }
}
