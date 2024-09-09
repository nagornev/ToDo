using System;
using System.Threading.Tasks;
using ToDo.Microservices.Identity.Domain.Models;

namespace ToDo.Microservices.Identity.UseCases.Repositories
{
    public interface IUserRepository
    {
        Task<User> Get(Guid userId);

        Task<User> Get(string email);

        Task<bool> Create(User user);

        Task<bool> Update(User user);

        Task<bool> Delete(Guid userId);
    }
}
