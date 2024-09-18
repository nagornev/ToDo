using System;
using System.Threading.Tasks;
using ToDo.Domain.Results;
using ToDo.Microservices.Categories.Domain.Models;

namespace ToDo.Microservices.Categories.UseCases.Services
{
    public interface IUserService
    {
        Task<Result<User>> GetUser(Guid userId);

        Task<Result> CreateUser(Guid userId);
    }
}
