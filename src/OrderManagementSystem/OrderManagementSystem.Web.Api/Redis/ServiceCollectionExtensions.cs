using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderManagementSystem.Core.Interface;
using OrderManagementSystem.Infrastructure.Services;
using StackExchange.Redis;

namespace OrderManagementSystem.Web.Api.Redis
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration config)
        {
            var redisConnStr = config.GetSection("Redis")["ConnectionString"];

            if (string.IsNullOrWhiteSpace(redisConnStr))
                throw new InvalidOperationException("Redis:ConnectionString is missing.");

            services.AddSingleton<IConnectionMultiplexer>(_ =>
                ConnectionMultiplexer.Connect(redisConnStr)
            );

            services.AddScoped<ICacheService, RedisCacheService>();

            return services;
        }
    }
}
