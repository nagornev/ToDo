using System.Runtime.CompilerServices;
using ToDo.Microservices.Entries.Infrastructure.Repositories;
using ToDo.Microservices.Entries.UseCases.Repositories;

namespace ToDo.Microservices.Entries.API.Extensions.Startup
{
    public static class RepositoriesStartupExtensions
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IEntryRepository, EntryRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
