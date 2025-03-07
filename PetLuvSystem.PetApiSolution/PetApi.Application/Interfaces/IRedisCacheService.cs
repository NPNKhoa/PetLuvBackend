namespace PetApi.Application.Interfaces
{
    public interface IRedisCacheService
    {
        public Task<string> GetCachedValueAsync(string key);
        public Task SetCachedValueAsync(string key, string value, TimeSpan? expiry = null);
    }
}
