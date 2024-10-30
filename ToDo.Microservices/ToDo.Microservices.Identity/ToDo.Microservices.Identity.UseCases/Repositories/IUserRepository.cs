using System;
using System.Threading.Tasks;
using ToDo.Domain.Results;
using ToDo.Microservices.Identity.Domain.Models;

namespace ToDo.Microservices.Identity.UseCases.Repositories
{
    public interface IUserRepository
    {
        Task<Result<User>> Get(Guid userId);

        Task<Result<User>> Get(string email);

        Task<Result> Create(User user);

        Task<Result> Update(User user);

        Task<Result> Delete(Guid userId);
    }
}
