using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDo.Microservices.Categories.Database.Entities;

namespace ToDo.Microservices.Categories.Database.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasMany(u => u.Categories)
                   .WithOne(c => c.User);
        }
    }
}
