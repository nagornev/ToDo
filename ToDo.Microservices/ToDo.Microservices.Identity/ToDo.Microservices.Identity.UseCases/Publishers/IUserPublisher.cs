using System.Threading.Tasks;
using ToDo.Domain.Results;
using ToDo.Microservices.Identity.Domain.Models;

namespace ToDo.Microservices.Identity.UseCases.Publishers
{
    public interface IUserPublisher
    {
        Task<Result> New(User user);
    }
}
