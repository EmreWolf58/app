using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OrderManagementSystem.Core.Interface;
using StackExchange.Redis;

namespace OrderManagementSystem.Infrastructure.Services
{
    public class RedisCacheService: ICacheService
    {
        private readonly IDatabase _db;
        private readonly ILogger<RedisCacheService> _logger;

        public RedisCacheService(IConnectionMultiplexer mux, ILogger<RedisCacheService> logger)
        {
            _db = mux.GetDatabase();
            _logger = logger;   
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            try
            {
                var val = await _db.StringGetAsync(key);
                if (!val.HasValue) return default;

                return JsonSerializer.Deserialize<T>(val!);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Redis GET failed. Key={Key}", key);
                return default;
            }
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan ttl)
        {
            try
            {
                var json = JsonSerializer.Serialize(value);
                await _db.StringSetAsync(key, json, ttl);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Redis SET failed. Key={Key}", key);
            }
        }

        public async Task RemoveAsync(string key)
        {
            try
            {
                await _db.KeyDeleteAsync(key);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Redis Remove Failed. Key={Key}", key);
            }
        }

        public async Task<long> GetVersionAsync(string versionKey)
        {
            try
            {
                var val= await _db.StringGetAsync(versionKey);
                if (!val.HasValue) return 1; //versiyon yoksa 1 kabul ediyoruz.

                return (long)val!;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Redis GetVersion Failed. VersionKey={VersionKey}", versionKey);
                return 1;
            }
        }

        public async Task<long> BumpVersionAsync (string versionKey)
        {
            try
            {
                // Atomic increment: aynı anda iki request gelse bile doğru artar.
                return await _db.StringIncrementAsync(versionKey);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Redis BumpVersion Failed. VersionKey={VersionKey}", versionKey);
                return 1;
            }
        }
    }
}
