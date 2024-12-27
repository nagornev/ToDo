using ToDo.Microservices.Categories.API.Middlewares;
using ToDo.Microservices.Middleware.Identities;

namespace ToDo.Microservices.Categories.API.Extensions.Startup
{
    public static class IdentityStartupExtensions
    {
        public static void AddIdentity(this IServiceCollection services)
        {
            services.AddIdentity<CategoriesIdentityAttributeProvider>();
        }
    }
}
