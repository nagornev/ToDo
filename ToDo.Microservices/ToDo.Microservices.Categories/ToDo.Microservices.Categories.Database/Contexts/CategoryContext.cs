using Microsoft.EntityFrameworkCore;
using ToDo.Microservices.Categories.Database.Configurations;
using ToDo.Microservices.Categories.Database.Entities;

namespace ToDo.Microservices.Categories.Database.Contexts
{
    public class CategoryContext : DbContext
    {
        public CategoryContext(DbContextOptions<CategoryContext> options)
            : base(options)
        {
        }

        public DbSet<UserEntity> Users { get; private set; }

        public DbSet<CategoryEntity> Categories { get; private set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        }
    }
}
