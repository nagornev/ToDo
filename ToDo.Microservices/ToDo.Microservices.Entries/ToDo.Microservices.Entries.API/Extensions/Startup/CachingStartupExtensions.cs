using StackExchange.Redis;
using ToDo.Microservices.Entries.Infrastructure.Cachers;

namespace ToDo.Microservices.Entries.API.Extensions.Startup
{
    public static class CachingStartupExtensions
    {
        public static void AddRedisCaching(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.ConnectionMultiplexerFactory =  async () => await ConnectionMultiplexer.ConnectAsync(configuration.GetConnectionString(nameof(EntryCacher))!);
            });
        }
    }
}
