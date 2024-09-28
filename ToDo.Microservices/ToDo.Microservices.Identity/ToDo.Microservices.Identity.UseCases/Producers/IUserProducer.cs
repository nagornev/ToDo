using System.Threading.Tasks;
using ToDo.Domain.Results;
using ToDo.Microservices.Identity.Domain.Models;

namespace ToDo.Microservices.Identity.UseCases.Producers
{
    public interface IUserProducer
    {
        Task<Result> New(User user);
    }
}
