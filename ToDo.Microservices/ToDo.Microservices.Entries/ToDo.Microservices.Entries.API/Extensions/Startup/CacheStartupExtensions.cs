﻿using StackExchange.Redis;
using ToDo.Microservices.Cache.Hashers;
using ToDo.Microservices.Entries.Infrastructure.Cachers;

namespace ToDo.Microservices.Entries.API.Extensions.Startup
{
    public static class CacheStartupExtensions
    {
        public static void AddCache(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCacheHashers();
            services.AddCacheHandlers();
            services.AddCacheDistributedClient(configuration);
        }

        private static void AddCacheHashers(this IServiceCollection services)
        {
            services.AddSingleton<EntryCacheHasher>();
            services.AddSingleton<CategoryCacheHasher>();
        }

        private static void AddCacheHandlers(this IServiceCollection services)
        {
            services.AddScoped<EntryCacheIO>();
            services.AddScoped<CategoryCacheReader>();
        }

        private static void AddCacheDistributedClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.ConnectionMultiplexerFactory = async () => await ConnectionMultiplexer.ConnectAsync(configuration.GetConnectionString(nameof(Cache))!);
            });
        }
    }
}
