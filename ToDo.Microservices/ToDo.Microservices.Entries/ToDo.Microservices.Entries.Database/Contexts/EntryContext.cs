using Microsoft.EntityFrameworkCore;
using ToDo.Microservices.Entries.Database.Configurations;
using ToDo.Microservices.Entries.Database.Entities;

namespace ToDo.Microservices.Entries.Database.Contexts
{
    public class EntryContext : DbContext
    {
        public EntryContext(DbContextOptions<EntryContext> options)
            : base(options)
        {
        }

        public DbSet<UserEntity> Users { get; private set; }

        public DbSet<EntryEntity> Entries { get; private set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new EntryConfiguration());
        }
    }
}
