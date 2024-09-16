using ToDo.Microservices.Identity.Infrastructure.Providers;
using ToDo.Microservices.Identity.UseCases.Providers;

namespace ToDo.Microservices.Identity.API.Extensions.Startup
{
    public static class ProvidersStartupExtesions
    {
        public static void AddProviders(this IServiceCollection services)
        {
            services.AddScoped<IHashProvider, PasswordHashProvider>();
            services.AddScoped<ITokenProvider, JwtTokenProvider>();
        }
    }
}
