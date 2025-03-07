namespace RoomApi.Application.Interfaces
{
    public interface IRedisCachingService
    {
        Task<string> GetCachedValueAsync(string key);
        Task SetCachedValueAsync(string key, string value, TimeSpan? expiry = null);
    }
}
