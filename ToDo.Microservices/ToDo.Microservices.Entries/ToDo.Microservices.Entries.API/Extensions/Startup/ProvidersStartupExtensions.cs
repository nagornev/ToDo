using ToDo.Microservices.Entries.Infrastructure.Providers;
using ToDo.Microservices.Entries.UseCases.Providers;

namespace ToDo.Microservices.Entries.API.Extensions.Startup
{
    public static class ProvidersStartupExtensions
    {
        public static void AddProviders(this IServiceCollection services)
        {
            services.AddScoped<IEntryСomposer, EntryComposer>();
        }
    }
}
