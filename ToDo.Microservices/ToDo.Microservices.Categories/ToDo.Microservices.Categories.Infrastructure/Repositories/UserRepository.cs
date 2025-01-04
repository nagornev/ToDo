using Microsoft.EntityFrameworkCore;
using ToDo.Domain.Results;
using ToDo.Microservices.Categories.Database.Contexts;
using ToDo.Microservices.Categories.Database.Entities;
using ToDo.Microservices.Categories.Domain.Models;
using ToDo.Microservices.Categories.UseCases.Repositories;
using ToDo.Microservices.Categories.Database.Extensions;

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
                        Result<User>.Successful(userEntity.GetDomain()) :
                        Result<User>.Failure(Errors.IsNull($"The user {userId} was not found."));
        }

        public async Task<Result> Create(User user)
        {
            await _context.Users.AddAsync(user.GetEntity());

            return await _context.SaveChangesAsync() > 0 ?
                        Result.Successful() :
                        Result.Failure(Errors.IsMessage("The user was not created. Please try again later"));
        }
    }
}
