using ToDo.Domain.Results;
using ToDo.Microservices.Identity.Domain.Models;
using ToDo.Microservices.Identity.UseCases.Producers;

namespace ToDo.Microservices.Identity.Infrastructure.Producers
{
    public class UserProducer : IUserProducer
    {
        public UserProducer()
        {
        }

        public async Task<Result> New(User user)
        {
            throw new NotImplementedException();
        }
    }
}
