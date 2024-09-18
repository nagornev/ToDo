using ToDo.Domain.Results;
using ToDo.Microservices.Categories.UseCases.Services;
using ToDo.Microservices.Middleware.Identities;

namespace ToDo.Microservices.Categories.API.Middlewares
{
    public class CategoriesIdentityMiddleware : IdentityMiddleware
    {
        public CategoriesIdentityMiddleware(RequestDelegate next)
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
