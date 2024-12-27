using ToDo.Microservices.Middleware.Identities;

namespace ToDo.Microservices.Entries.API.Middlewares
{
    public class EntriesIdentityAttributeProvider : IIdentityAttributeProvider
    {
        public bool TryGet(HttpContext context, out IdentityAttribute attribute)
        {
            attribute = context.GetEndpoint()?
                             .Metadata
                             .GetMetadata<IdentityAttribute>();

            return attribute is not null;
        }
    }
}
