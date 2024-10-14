using ToDo.Microservices.Entries.Infrastructure.Repositories;
using ToDo.Microservices.Entries.UseCases.Repositories;

namespace ToDo.Microservices.Entries.API.Extensions.Startup
{
    public static class RepositoriesStartupExtensions
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IEntryRepository, CachedEntryRepository>();
            services.AddScoped<ICategoryRepository, CachedCategoryRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
