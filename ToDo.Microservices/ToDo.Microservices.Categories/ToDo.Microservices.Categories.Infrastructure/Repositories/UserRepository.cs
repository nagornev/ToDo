using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<User?> Get(Guid userId)
        {
            UserEntity? userEntity = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);

            return userEntity is not null ?
                    User.Constructor(userEntity.Id) :
                    default;
        }

        public async Task<bool> Create(User user)
        {
            UserEntity userEntity = new UserEntity()
            {
                Id = user.Id,
            };

            await _context.Users.AddAsync(userEntity);

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
    }
}
