using System;
using System.Threading.Tasks;
using ToDo.Microservices.Entries.Domain.Models;

namespace ToDo.Microservices.Entries.UseCases.Repositories
{
    public interface IUserRepository
    {
        Task<User> Get(Guid userId);

        Task<bool> Create(User user);
    }
}
