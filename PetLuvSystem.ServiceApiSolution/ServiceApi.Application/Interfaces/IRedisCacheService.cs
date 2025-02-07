namespace ServiceApi.Application.Interfaces
{
    public interface IRedisCacheService
    {
        Task<string> GetCachedValueAsync(string key);
        Task SetCachedValueAsync(string key, string value, TimeSpan? expiry = null);
    }
}
