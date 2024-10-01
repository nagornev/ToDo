using Microsoft.EntityFrameworkCore;
using ToDo.Microservices.Categories.Database.Contexts;
using ToDo.Microservices.Categories.Database.Entities;
using ToDo.Microservices.Categories.Domain.Models;
using ToDo.Microservices.Categories.UseCases.Repositories;

namespace ToDo.Microservices.Categories.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private CategoryContext _context;

        public CategoryRepository(CategoryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> Get(Guid userId)
        {
            IEnumerable<CategoryEntity> categoryEntities = await _context.Categories.AsNoTracking()
                                                                                    .Where(x => x.UserId == userId)
                                                                                    .ToListAsync();

            IEnumerable<Category> categories = categoryEntities.Select(x => Category.Constructor(x.Id, x.Name));

            return categories;
        }

        public async Task<Category?> Get(Guid userId, Guid categoryId)
        {
            CategoryEntity? categoryEntity = await _context.Categories.AsNoTracking()
                                                                      .FirstOrDefaultAsync(x => x.UserId == userId &&
                                                                                                x.Id == categoryId);

            return categoryEntity is not null ?
                    Category.Constructor(categoryEntity.Id, categoryEntity.Name) :
                    default;
        }


        public async Task<bool> Create(Guid userId, Category category)
        {
            CategoryEntity categoryEntity = new CategoryEntity()
            {
                Id = category.Id,
                Name = category.Name,

                UserId = userId,
            };

            await _context.Categories.AddAsync(categoryEntity);

            try
            {
                int rows = await _context.SaveChangesAsync();
                return rows > 0;
            }
            catch
            {
                return false;
            }
        }


        public async Task<bool> Update(Guid userId, Category category)
        {

            int rows = await _context.Categories.Where(x => x.UserId == userId &&
                                                            x.Id == category.Id)
                                                .ExecuteUpdateAsync(x => x.SetProperty(p => p.Name, category.Name));

            return rows > 0;
        }

        public async Task<bool> Delete(Guid userId, Guid categoryId)
        {
            int rows = await _context.Categories.Where(x => x.UserId == userId &&
                                                           x.Id == categoryId)
                                               .ExecuteDeleteAsync();

            return rows > 0;
        }
    }
}
