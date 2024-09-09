using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDo.Microservices.Identity.Database.Entities;

namespace ToDo.Microservices.Identity.Database.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasIndex(u => u.Email)
                   .IsUnique();

            builder.HasOne(u => u.Role)
                   .WithMany(r => r.Users);
        }
    }
}
