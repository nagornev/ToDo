using ToDo.Microservices.Entries.Infrastructure.Services;
using ToDo.Microservices.Entries.UseCases.Services;

namespace ToDo.Microservices.Entries.API.Extensions.Startup
{
    public static class ServicesStartupExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IEntryService, EntryService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICategoryService, CategoryService>();
        }
    }
}
