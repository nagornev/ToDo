using ToDo.Domain.Results;

namespace ToDo.Microservices.Middleware.Identities
{
    public interface IIdentityChecker
    {
        Task<Result> Check(Guid userId);
    }
}
