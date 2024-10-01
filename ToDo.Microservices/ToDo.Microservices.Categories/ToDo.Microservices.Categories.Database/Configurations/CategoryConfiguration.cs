using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDo.Microservices.Categories.Database.Entities;

namespace ToDo.Microservices.Categories.Database.Configurations
{
    internal class CategoryConfiguration : IEntityTypeConfiguration<CategoryEntity>
    {
        public void Configure(EntityTypeBuilder<CategoryEntity> builder)
        {
            builder.HasOne(c => c.User)
                   .WithMany(u => u.Categories);
        }
    }
}
