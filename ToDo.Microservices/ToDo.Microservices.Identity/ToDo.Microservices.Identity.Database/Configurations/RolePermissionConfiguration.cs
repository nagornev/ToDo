using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDo.Microservices.Identity.Database.Entities;
using ToDo.Microservices.Identity.Domain.Models;

namespace ToDo.Microservices.Identity.Database.Configurations
{
    internal class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermissionEntity>
    {
        public void Configure(EntityTypeBuilder<RolePermissionEntity> builder)
        {
            builder.HasKey(rp => new { rp.RoleId, rp.PermissionId });

            builder.HasData(GetData());
        }

        private IEnumerable<RolePermissionEntity> GetData()
        {
            List<RolePermissionEntity> rolePermissionEntities = new List<RolePermissionEntity>();

            foreach (Role role in Enum.GetValues<Role>())
            {
                foreach (Permission permission in Access.Constructor(role).Permissions)
                {
                    RolePermissionEntity rolePermissionEntity = new RolePermissionEntity()
                    {
                        RoleId = (int)role,
                        PermissionId = (int)permission
                    };

                    rolePermissionEntities.Add(rolePermissionEntity);
                }
            }

            return rolePermissionEntities;
        }
    }
}
