using System;
using System.Threading.Tasks;
using ToDo.Domain.Results;
using ToDo.Microservices.Categories.Domain.Models;

namespace ToDo.Microservices.Categories.UseCases.Repositories
{
    public interface IUserRepository
    {
        Task<Result<User>> Get(Guid userId);

        Task<Result> Create(User user);
    }
}
