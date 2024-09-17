using Microsoft.EntityFrameworkCore;
using ToDo.Microservices.Entries.Database.Contexts;

namespace ToDo.Microservices.Entries.API.Extensions.Startup
{
    public static class ContextsStartupExtensions
    {
        public static void AddContexts(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<EntryContext>(options => options.UseSqlServer(configuration.GetConnectionString(nameof(EntryContext))));
        }
    }
}
