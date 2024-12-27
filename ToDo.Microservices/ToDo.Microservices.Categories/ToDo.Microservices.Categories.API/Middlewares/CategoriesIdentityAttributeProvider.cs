using ToDo.Microservices.Middleware.Identities;

namespace ToDo.Microservices.Categories.API.Middlewares
{
    public class CategoriesIdentityAttributeProvider : IIdentityAttributeProvider
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
