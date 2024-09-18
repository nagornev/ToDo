using System;
using System.Threading.Tasks;
using ToDo.Microservices.Categories.Domain.Models;

namespace ToDo.Microservices.Categories.UseCases.Repositories
{
    public interface IUserRepository
    {
        Task<User> Get(Guid userId);

        Task<bool> Create(User user);
    }
}
