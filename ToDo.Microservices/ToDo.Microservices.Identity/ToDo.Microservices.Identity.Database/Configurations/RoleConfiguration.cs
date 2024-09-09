using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDo.Microservices.Identity.Database.Entities;
using ToDo.Microservices.Identity.Domain.Models;

namespace ToDo.Microservices.Identity.Database.Configurations
{
    internal class RoleConfiguration : IEntityTypeConfiguration<RoleEntity>
    {
        public void Configure(EntityTypeBuilder<RoleEntity> builder)
        {
            builder.HasMany(r => r.Users)
                   .WithOne(u => u.Role);


            builder.HasMany(r => r.Permissions)
                   .WithMany(p => p.Roles)
                   .UsingEntity<RolePermissionEntity>(
                    l => l.HasOne<PermissionEntity>().WithMany().HasForeignKey(rp => rp.PermissionId),
                    r => r.HasOne<RoleEntity>().WithMany().HasForeignKey(rp => rp.RoleId));

            builder.HasData(GetData());
        }

        private IEnumerable<RoleEntity> GetData()
        {
            List<RoleEntity> roleEntities = new List<RoleEntity>();

            foreach (Role role in Enum.GetValues<Role>())
            {
                RoleEntity roleEntity = new RoleEntity()
                {
                    Id = (int)role,
                    Name = role.ToString()
                };

                roleEntities.Add(roleEntity);
            }

            return roleEntities;
        }
    }
}
