using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Microservices.Identity.Database.Entities;
using ToDo.Microservices.Identity.Domain.Models;

namespace ToDo.Microservices.Identity.Database.Configurations
{
    internal class PermissionConfiguration : IEntityTypeConfiguration<PermissionEntity>
    {
        public void Configure(EntityTypeBuilder<PermissionEntity> builder)
        {
            builder.HasMany(p => p.Roles)
                   .WithMany(p => p.Permissions)
                   .UsingEntity<RolePermissionEntity>(
                    l => l.HasOne<RoleEntity>().WithMany().HasForeignKey(rp => rp.RoleId),
                    r => r.HasOne<PermissionEntity>().WithMany().HasForeignKey(rp => rp.PermissionId));

            builder.HasData(GetData());
        }

        private IEnumerable<PermissionEntity> GetData()
        {
            List<PermissionEntity> permissionEntities = new List<PermissionEntity>();

            foreach (Permission permission in Enum.GetValues<Permission>())
            {
                PermissionEntity permissionEntity = new PermissionEntity()
                {
                    Id = (int)permission,
                    Name = permission.ToString()
                };

                permissionEntities.Add(permissionEntity);
            }

            return permissionEntities;
        }
    }
}
