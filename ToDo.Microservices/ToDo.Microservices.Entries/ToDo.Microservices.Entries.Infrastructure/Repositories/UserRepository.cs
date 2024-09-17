using Microsoft.EntityFrameworkCore;
using ToDo.Microservices.Entries.Database.Contexts;
using ToDo.Microservices.Entries.Database.Entities;
using ToDo.Microservices.Entries.Domain.Models;
using ToDo.Microservices.Entries.UseCases.Repositories;

namespace ToDo.Microservices.Entries.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private EntryContext _entryContext;

        public UserRepository(EntryContext entryContext)
        {
            _entryContext = entryContext;
        }

        public async Task<User?> Get(Guid userId)
        {
            UserEntity? userEntity = await _entryContext.Users.AsNoTracking()
                                                              .FirstOrDefaultAsync(x => x.Id == userId);

            User? user = userEntity is not null ?
                            User.Constructor(userEntity.Id) :
                            default;

            return user;
        }

        public async Task<bool> Create(User user)
        {
            UserEntity userEntity = new UserEntity()
            {
                Id = user.Id,
            };

            await _entryContext.Users.AddAsync(userEntity);

            try
            {
                int rows = await _entryContext.SaveChangesAsync();
                return rows > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
