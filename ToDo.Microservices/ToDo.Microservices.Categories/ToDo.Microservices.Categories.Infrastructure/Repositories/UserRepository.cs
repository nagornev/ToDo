using Microsoft.EntityFrameworkCore;
using ToDo.Domain.Results;
using ToDo.Microservices.Categories.Database.Contexts;
using ToDo.Microservices.Categories.Database.Entities;
using ToDo.Microservices.Categories.Domain.Models;
using ToDo.Microservices.Categories.UseCases.Repositories;

namespace ToDo.Microservices.Categories.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private CategoryContext _context;

        public UserRepository(CategoryContext context)
        {
            _context = context;
        }

        public async Task<Result<User>> Get(Guid userId)
        {
            UserEntity? userEntity = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);

            return userEntity is not null ?
                        Result<User>.Successful(User.Constructor(userEntity.Id)) :
                        Result<User>.Failure(Errors.IsNull($"The user {userId} was not found."));
        }

        public async Task<Result> Create(User user)
        {
            UserEntity userEntity = CreateUserEntity(user);

            await _context.Users.AddAsync(userEntity);

            return await _context.SaveChangesAsync() > 0 ?
                        Result.Successful() :
                        Result.Failure(Errors.IsMessage("The user was not created. Please try again later"));
        }

        private UserEntity CreateUserEntity(User user)
        {
            return new UserEntity()
            {
                Id = user.Id,
            };
        }
    }
}
