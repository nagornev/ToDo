using ToDo.Microservices.Categories.Infrastructure.Services;
using ToDo.Microservices.Categories.UseCases.Services;

namespace ToDo.Microservices.Categories.API.Extensions.Startup
{
    public static class ServicesStartupExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICategoryService, CategoryService>();
        }
    }
}
