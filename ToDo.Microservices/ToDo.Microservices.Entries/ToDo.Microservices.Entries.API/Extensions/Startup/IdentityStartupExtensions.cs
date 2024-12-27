using ToDo.Microservices.Entries.API.Middlewares;
using ToDo.Microservices.Middleware.Identities;

namespace ToDo.Microservices.Entries.API.Extensions.Startup
{
    public static class IdentityStartupExtensions
    {
        public static void AddIdentity(this IServiceCollection services)
        {
            services.AddIdentity<EntriesIdentityAttributeProvider>();
        }
    }
}
