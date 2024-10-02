using System;
using System.Threading.Tasks;
using ToDo.Domain.Results;
using ToDo.Microservices.Entries.Domain.Models;

namespace ToDo.Microservices.Entries.UseCases.Repositories
{
    public interface IUserRepository
    {
        Task<Result<User>> Get(Guid userId);

        Task<Result> Create(User user);
    }
}
