﻿using Microsoft.Extensions.Configuration;
using PetLuvSystem.SharedLibrary.Logs;
using StackExchange.Redis;
using UserApi.Application.Interfaces;

namespace UserApi.Infrastructure.Services
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDatabase _cache;

        public RedisCacheService(IConfiguration configuration)
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
