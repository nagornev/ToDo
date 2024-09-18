using Microsoft.EntityFrameworkCore;
using ToDo.Microservices.Categories.Database.Contexts;

namespace ToDo.Microservices.Categories.API.Extensions.Startup
{
    public static class ContextsStartupExtensions
    {
        public static void AddContexts(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CategoryContext>(options => options.UseSqlServer(configuration.GetConnectionString(nameof(CategoryContext))));
        }
    }
}
