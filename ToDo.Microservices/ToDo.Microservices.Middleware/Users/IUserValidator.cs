using ToDo.Domain.Results;

namespace ToDo.Microservices.Middleware.Users
{
    public interface IUserValidator
    {
        Task<Result> Validate(Guid userId);
    }
}
