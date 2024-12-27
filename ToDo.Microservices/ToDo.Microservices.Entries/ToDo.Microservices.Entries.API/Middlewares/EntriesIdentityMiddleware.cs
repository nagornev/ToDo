using ToDo.Microservices.Middleware.Identities;

namespace ToDo.Microservices.Entries.API.Middlewares
{
    public class EntriesIdentityMiddleware : IdentityMiddleware
    {
        public EntriesIdentityMiddleware(RequestDelegate next)
            : base(next)
        {
        }

        protected override bool TryGetIdentity(HttpContext context, out IdentityAttribute? attribute)
        {
            attribute = context.GetEndpoint()?
                               .Metadata
                               .GetMetadata<IdentityAttribute>();

            return attribute is not null;
        }
    }
}
