using Microsoft.Extensions.Configuration;
using PetLuvSystem.SharedLibrary.Logs;
using RoomApi.Application.Interfaces;
using StackExchange.Redis;

namespace RoomApi.Infrastructure.Services
{
    public class RedisCachingService : IRedisCachingService
    {
        private readonly IDatabase _cache;

        public RedisCachingService(IConfiguration configuration)
        {
            var redisConnection = ConnectionMultiplexer.Connect(configuration["Redis:ConnectionString"]!);
            _cache = redisConnection.GetDatabase();
        }

        public async Task<string> GetCachedValueAsync(string key)
        {
            try
            {
                return await _cache.StringGetAsync(key);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error getting value from Redis: ");
                LogException.LogExceptions(ex);
                return null;
            }
        }

        public async Task SetCachedValueAsync(string key, string value, TimeSpan? expiry = null)
        {
            try
            {
                await _cache.StringSetAsync(key, value, expiry);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error setting value in Redis: ");
                LogException.LogExceptions(ex);
            }
        }
    }
}
