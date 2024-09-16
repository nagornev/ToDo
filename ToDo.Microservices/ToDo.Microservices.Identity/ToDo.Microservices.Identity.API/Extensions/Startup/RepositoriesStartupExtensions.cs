using ToDo.Microservices.Identity.Infrastructure.Repositories;
using ToDo.Microservices.Identity.UseCases.Repositories;

namespace ToDo.Microservices.Identity.API.Extensions.Startup
{
    public static class RepositoriesStartupExtensions
    {
        public static void AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
