using ToDo.Domain.Results;
using ToDo.Microservices.Categories.UseCases.Services;
using ToDo.Microservices.Middleware.Identities;

namespace ToDo.Microservices.Categories.API.Middlewares
{
    public class CategoryIdentityMiddleware : IdentityMiddleware
    {
        private IUserService _userServie;

        public CategoryIdentityMiddleware(RequestDelegate next, IUserService userServie) 
            : base(next)
        {
            _userServie = userServie;
        }

        protected override bool TryGetIdentity(HttpContext context, out IdentityAttribute? attribute)
        {
            attribute = context.GetEndpoint()?
                               .Metadata
                               .GetMetadata<IdentityAttribute>();

            return attribute is not null;
        }

        protected async override Task<Result> Check(Guid userId)
        {
            return await _userServie.GetUser(userId);
        }
    }
}
