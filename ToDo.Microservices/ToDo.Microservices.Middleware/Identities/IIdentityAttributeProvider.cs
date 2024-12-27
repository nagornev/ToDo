using Microsoft.AspNetCore.Http;

namespace ToDo.Microservices.Middleware.Identities
{
    public interface IIdentityAttributeProvider
    {
        bool TryGet(HttpContext context, out IdentityAttribute attribute);
    }
}
