namespace OrderManagementSystem.Core.Interface
{
    public interface ICacheService
    {
        Task<T?> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan ttl);
        Task RemoveAsync(string key);

        // “Versioned cache” için: ürün listesi cache key’ine versiyon ekleyeceğiz.
        Task<long> GetVersionAsync(string versionKey);
        Task<long> BumpVersionAsync(string versionKey);
    }
}
