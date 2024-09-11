using ToDo.Microservices.Identity.Infrastructure.Services;
using ToDo.Microservices.Identity.UseCases.Services;

namespace ToDo.Microservices.Identity.API.Extensions.Stratup
{
    public static class ServicesStartupExtensions
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserService, UserService>();
        }
    }
}
