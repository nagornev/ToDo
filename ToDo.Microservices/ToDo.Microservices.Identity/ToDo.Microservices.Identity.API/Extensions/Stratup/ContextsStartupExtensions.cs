using Microsoft.EntityFrameworkCore;
using ToDo.Microservices.Identity.Database.Contexts;

namespace ToDo.Microservices.Identity.API.Extensions.Stratup
{
    public static class ContextsStartupExtensions
    {
        public static void AddContexts(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IdentityContext>(options => options.UseSqlServer(configuration.GetConnectionString(nameof(IdentityContext))));
        }
    }
}
