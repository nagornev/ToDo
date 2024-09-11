using ToDo.Microservices.Identity.Infrastructure.Providers;
using ToDo.Microservices.Identity.UseCases.Providers;

namespace ToDo.Microservices.Identity.API.Extensions.Stratup
{
    public static class ProvidersServicesExtesions
    {
        public static void AddProviders(this IServiceCollection services)
        {
            services.AddScoped<IHashProvider, PasswordHashProvider>();
            services.AddScoped<ITokenProvider, JwtTokenProvider>();
        }
    }
}
