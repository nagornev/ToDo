using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDo.Microservices.Entries.Database.Entities;

namespace ToDo.Microservices.Entries.Database.Configurations
{
    public class EntryConfiguration : IEntityTypeConfiguration<EntryEntity>
    {
        public void Configure(EntityTypeBuilder<EntryEntity> builder)
        {
            builder.HasOne(e => e.User)
                   .WithMany(u => u.Entries);
        }
    }
}
