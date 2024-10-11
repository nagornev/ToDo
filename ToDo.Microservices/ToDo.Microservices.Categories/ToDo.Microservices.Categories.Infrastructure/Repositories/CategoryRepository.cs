using Microsoft.EntityFrameworkCore;
using ToDo.Domain.Results;
using ToDo.Microservices.Categories.Database.Contexts;
using ToDo.Microservices.Categories.Database.Entities;
using ToDo.Microservices.Categories.Domain.Models;
using ToDo.Microservices.Categories.UseCases.Publishers;
using ToDo.Microservices.Categories.UseCases.Repositories;

namespace ToDo.Microservices.Categories.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private CategoryContext _context;

        private ICategoryPubliser _categoryPublisher;

        public CategoryRepository(CategoryContext context,
                                  ICategoryPubliser categoryPublisher)
        {
            _context = context;
            _categoryPublisher = categoryPublisher;
        }

        public async Task<Result<IEnumerable<Category>>> Get(Guid userId)
        {
            IEnumerable<CategoryEntity> categoryEntities = await _context.Categories.AsNoTracking()
                                                                                        .Where(x => x.UserId == userId)
                                                                                        .ToListAsync();

            IEnumerable<Category> categories = categoryEntities.Select(x => Category.Constructor(x.Id, x.Name));

            return Result<IEnumerable<Category>>.Successful(categories);
        }

        public async Task<Result<Category>> Get(Guid userId, Guid categoryId)
        {
            CategoryEntity? categoryEntity = await _context.Categories.AsNoTracking()
                                                                      .FirstOrDefaultAsync(x => x.UserId == userId &&
                                                                                                x.Id == categoryId);

            return categoryEntity is not null ?
                    Result<Category>.Successful(Category.Constructor(categoryEntity.Id, categoryEntity.Name)) :
                    Result<Category>.Failure(Errors.IsNull($"The category {categoryId} was not found."));
        }


        public async Task<Result> Create(Guid userId, Category category)
        {
            CategoryEntity categoryEntity = CreateCategoryEntity(userId, category);

            await _context.Categories.AddAsync(categoryEntity);

            return await _context.SaveChangesAsync() > 0 ?
                     Result.Successful() :
                     Result.Failure(Errors.IsMessage("The category was not created. Please check category parameters and try again later."));
        }


        public async Task<Result> Update(Guid userId, Category category)
        {
            return await _context.Categories.Where(x => x.UserId == userId &&
                                                            x.Id == category.Id)
                                            .ExecuteUpdateAsync(x => x.SetProperty(p => p.Name, category.Name)) > 0 ?
                      Result.Successful() :
                      Result.Failure(Errors.IsNull($"The category {category.Id} was not found."));
        }

        public async Task<Result> Delete(Guid userId, Guid categoryId)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    CategoryEntity? categoryEntity = await _context.Categories.FirstOrDefaultAsync(x => x.UserId == userId &&
                                                                                                        x.Id == categoryId);

                    if (categoryEntity is null)
                        return Result.Failure(Errors.IsNull($"The category {categoryId} was not found."));

                    _context.Categories.Remove(categoryEntity);
                    await _context.SaveChangesAsync();
                    await _categoryPublisher.Delete(userId, categoryId);

                    transaction.Commit();

                    return Result.Successful();
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        private CategoryEntity CreateCategoryEntity(Guid userId, Category category)
        {
            return new CategoryEntity()
            {
                Id = category.Id,
                Name = category.Name,

                UserId = userId,
            };
        }
    }
}
