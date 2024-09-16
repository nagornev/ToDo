using Microsoft.Extensions.Options;
using ToDo.Microservices.Identity.Infrastructure.Providers;

namespace ToDo.Microservices.Identity.API.Extensions.Startup
{
    public static class OptionsStartupExtesions
    {
        public static void AddOptions(this IServiceCollection services, IConfiguration configuration)
        {
            IOptions<PasswordHashProviderOptions> passwordOptions = Options.Create(configuration.GetSection(nameof(PasswordHashProviderOptions))
                                                                                    .Get<PasswordHashProviderOptions>()!);

            IOptions<JwtTokenProviderOptions> jwtOptions = Options.Create(configuration.GetSection(nameof(JwtTokenProviderOptions))
                                                                                       .Get<JwtTokenProviderOptions>()!);


            services.AddSingleton(passwordOptions);
            services.AddSingleton(jwtOptions);
        }
    }
}
