using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDo.Domain.Results;
using ToDo.Microservices.Identity.Domain.Models;

namespace ToDo.Microservices.Identity.UseCases.Services
{
    public interface IUserService
    {
        Task<Result<User>> GetUser(Guid userId);

        Task<Result<User>> GetUser(string email);

        Task<Result> SignUp(string email, string password);

        Task<Result<string>> SignIn(string email, string password);

        Task<Result<Guid>> Validate(string token, IEnumerable<Permission> permissions);
    }
}
