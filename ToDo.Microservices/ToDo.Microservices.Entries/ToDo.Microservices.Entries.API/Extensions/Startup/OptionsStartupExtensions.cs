using Microsoft.Extensions.Options;
using ToDo.Microservices.Entries.Infrastructure.Options;

namespace ToDo.Microservices.Entries.API.Extensions.Startup
{
    public static class OptionsStartupExtensions
    {
        public static void AddOptions(this IServiceCollection services, IConfiguration configuration)
        {
            IOptions<CategoryServiceOptions> categoryOptions = Options.Create(configuration.GetSection(nameof(CategoryServiceOptions))
                                                                                           .Get<CategoryServiceOptions>()!);

            services.AddSingleton(categoryOptions);
        }
    }
}
