using System;
using System.Threading.Tasks;
using ToDo.Domain.Results;
using ToDo.Microservices.Entries.Domain.Models;

namespace ToDo.Microservices.Entries.UseCases.Services
{
    public interface IUserService
    {
        Task<Result<User>> GetUser(Guid userId);

        Task<Result> CreateUser(Guid userId);
    }
}
