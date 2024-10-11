using ToDo.Microservices.Entries.Infrastructure.Cachers;
using ToDo.Microservices.Entries.UseCases.Cachers;

namespace ToDo.Microservices.Entries.API.Extensions.Startup
{
    public static class CachersStartupExtensions
    {
        public static void AddCachers(this IServiceCollection services)
        {
            services.AddScoped<IEntryCacher, EntryCacher>();
        }
    }
}
