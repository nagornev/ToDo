using Microsoft.EntityFrameworkCore;
using ToDo.Domain.Results;
using ToDo.Domain.Results.Extensions;
using ToDo.Microservices.Categories.Database.Contexts;
using ToDo.Microservices.Categories.Database.Entities;
using ToDo.Microservices.Categories.Database.Extensions;
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
            UserEntity? userEntity = await _context.Users.AsNoTracking()
                                                         .Include(x => x.Categories)
                                                         .FirstOrDefaultAsync(x => x.Id == userId);

            return userEntity is not null ?
                    userEntity.Categories.GetDomain() :
                    Result<IEnumerable<Category>>.Failure(error => error.NullOrEmpty($"The user {userId} was not found."));
        }

        public async Task<Result<Category>> Get(Guid userId, Guid categoryId)
        {
            CategoryEntity? categoryEntity = await _context.Categories.AsNoTracking()
                                                                      .FirstOrDefaultAsync(x => x.UserId == userId &&
                                                                                                x.Id == categoryId);

            return categoryEntity is not null ?
                        categoryEntity.GetDomain() :
                        Result<Category>.Failure(error => error.NullOrEmpty($"The category {categoryId} was not found. Please check get category parameters and try again later."));
        }


        public async Task<Result> Create(Guid userId, Category category)
        {
            await _context.Categories.AddAsync(category.GetEntity(userId));

            return await _context.SaveChangesAsync() > 0 ?
                     Result.Successful() :
                     Result.Failure(error => error.InternalServer($"The category \"{category.Name}\" was not created. Please check get category parameters and try again later."));
        }


        public async Task<Result> Update(Guid userId, Category category)
        {
            return await _context.Categories.Where(x => x.UserId == userId &&
                                                        x.Id == category.Id)
                                            .ExecuteUpdateAsync(x => x.SetProperty(p => p.Name, category.Name)) > 0 ?
                      Result.Successful() :
                      Result.Failure(error => error.InternalServer($"The category {category.Id} was not updated. Please check update parameters and try again later."));
        }

        public async Task<Result> Delete(Guid userId, Guid categoryId)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (((await _context.Categories.Where(x => x.UserId == userId &&
                                                              x.Id == categoryId)
                                                   .ExecuteDeleteAsync()) > 0)
                        &&
                        (await _categoryPublisher.Delete(userId, categoryId)).Success)
                    {
                        await transaction.CommitAsync();
                        return Result.Successful();
                    }

                    await transaction.RollbackAsync();
                    return Result.Failure(error => error.NullOrEmpty($"The category {categoryId} was not deleted. Please check delete parameters and try again later."));
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}
