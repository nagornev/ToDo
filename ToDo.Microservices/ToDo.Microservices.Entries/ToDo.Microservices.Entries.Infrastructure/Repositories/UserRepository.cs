using Microsoft.EntityFrameworkCore;
using ToDo.Domain.Results;
using ToDo.Microservices.Entries.Database.Contexts;
using ToDo.Microservices.Entries.Database.Entities;
using ToDo.Microservices.Entries.Database.Extensions;
using ToDo.Microservices.Entries.Domain.Models;
using ToDo.Microservices.Entries.UseCases.Repositories;

namespace ToDo.Microservices.Entries.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private EntryContext _context;

        public UserRepository(EntryContext entryContext)
        {
            _context = entryContext;
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
