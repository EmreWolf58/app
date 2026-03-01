using OrderManagementSystem.Core.Interface;

namespace OrderManagementSystem.Infrastructure.Services
{
    public class NullCacheService : ICacheService
    {
        public Task<T?> GetAsync<T>(string key)
        {
            return Task.FromResult<T?>(default);
        }

        public Task SetAsync<T>(string key, T value, TimeSpan ttl)
        {
            return Task.CompletedTask;
        }

        public Task RemoveAsync(string key)
        {
            return Task.CompletedTask;
        }

        public Task<long> GetVersionAsync(string versionKey)
        {
            return Task.FromResult(0L);
        }

        public Task<long> BumpVersionAsync(string versionKey)
        {
            return Task.FromResult(1L);
        }
    }
}