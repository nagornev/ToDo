using System.Security.Claims;

namespace ToDo.Microservices.Middleware.Identities
{
    public static class IdentityExtensions
    {
        public static Guid GetId(this ClaimsPrincipal principal)
        {
            string id = principal.Claims.First(x => x.Type == IdentityDefaults.Subject).Value;

            return Guid.Parse(id);
        }
    }
}
