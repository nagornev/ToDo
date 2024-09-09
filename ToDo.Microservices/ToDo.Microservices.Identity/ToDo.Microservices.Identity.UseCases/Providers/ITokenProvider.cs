using ToDo.Microservices.Identity.Domain.Models;

namespace ToDo.Microservices.Identity.UseCases.Providers
{
    public interface ITokenProvider
    {
        string Create(User user);

        bool Validate(string token, out string subject);
    }
}
