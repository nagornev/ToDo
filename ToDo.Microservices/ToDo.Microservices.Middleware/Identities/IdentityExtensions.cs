using Nagornev.Querer.Http;
using Nagornev.Querer.Http.Loggers;
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

        public static QuererLoggerBuilder AddAspLogger(this QuererLoggerBuilder builder)
        {
            return builder.AddLogger(new LoggerBridge<QuererLoggerBuilder>());
        }
    }
}
