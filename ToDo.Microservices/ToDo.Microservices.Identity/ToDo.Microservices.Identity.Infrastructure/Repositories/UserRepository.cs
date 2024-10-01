using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ToDo.Microservices.Identity.Database.Contexts;
using ToDo.Microservices.Identity.Database.Entities;
using ToDo.Microservices.Identity.Domain.Models;
using ToDo.Microservices.Identity.UseCases.Repositories;

namespace ToDo.Microservices.Identity.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private IdentityContext _context;

        public UserRepository(IdentityContext context)
        {
            _context = context;
        }

        public async Task<User?> Get(Guid userId)
        {
            return await GetUser(x => x.Id == userId);
        }

        public async Task<User?> Get(string email)
        {
            return await GetUser(x => x.Email == email);
        }

        public async Task<bool> Create(User user)
        {
            UserEntity userEntity = new UserEntity()
            {
                Id = user.Id,
                Email = user.Email,
                Password = user.Password,
                RoleId = (int)user.Access.Role
            };

            await _context.Users.AddAsync(userEntity);

            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<bool> Update(User user)
        {
            return (await _context.Users.Where(x => x.Id == user.Id)
                                            .ExecuteUpdateAsync(p => p.SetProperty(x => x.Email, user.Email)
                                                                      .SetProperty(x => x.RoleId, (int)user.Access.Role))) > 0;
        }

        public async Task<bool> Delete(Guid userId)
        {
            return (await _context.Users.Where(x => x.Id == userId)
                                        .ExecuteDeleteAsync()) > 0;
        }

        private async Task<User?> GetUser(Expression<Func<UserEntity, bool>> predicate)
        {
            UserEntity? userEntity = await _context.Users.FirstOrDefaultAsync(predicate);

            return userEntity is not null ?
                    User.Constructor(userEntity.Id,
                             userEntity.Email,
                             userEntity.Password,
                             Access.Constructor((Role)userEntity.RoleId)) :
                    default;
        }
    }
}
