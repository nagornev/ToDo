using ToDo.Microservices.Categories.Infrastructure.Repositories;
using ToDo.Microservices.Categories.UseCases.Repositories;

namespace ToDo.Microservices.Categories.API.Extensions.Startup
{
    public static class RepositoriesStartupExtensions
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<CategoryRepository>();
            services.AddScoped<ICategoryRepository, CachedCategoryRepository>();

            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
