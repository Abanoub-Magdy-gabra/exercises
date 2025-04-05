using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace Workout.Infrastructure.Caching
{
    public interface IRedisCacheService
    {
        Task<T> GetAsync<T>(string key);
        Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiry = null);
        Task<bool> RemoveAsync(string key);
        Task<bool> ExistsAsync(string key);
        Task<bool> RemoveByPatternAsync(string pattern);
    }

    public class RedisCacheService : IRedisCacheService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly StackExchange.Redis.IDatabase _db;
        private readonly ILogger<RedisCacheService> _logger;
        private readonly bool _enabled;
        private readonly TimeSpan _defaultExpiry;

        public RedisCacheService(IConfiguration configuration, ILogger<RedisCacheService> logger)
        {
            _logger = logger;

            var cacheSection = configuration.GetSection("Cache:Redis");
            _enabled = cacheSection.GetValue<bool>("Enabled");
            _defaultExpiry = TimeSpan.FromMinutes(cacheSection.GetValue("DefaultExpiryMinutes", 30));

            if (_enabled)
            {
                try
                {
                    var connectionString = cacheSection.GetValue<string>("ConnectionString");
                    if (string.IsNullOrEmpty(connectionString))
                    {
                        _logger.LogWarning("Redis cache enabled but no connection string provided. Disabling cache.");
                        _enabled = false;
                        return;
                    }

                    _redis = ConnectionMultiplexer.Connect(connectionString);
                    _db = _redis.GetDatabase();
                    _logger.LogInformation("Redis cache initialized successfully.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to initialize Redis cache. Disabling cache.");
                    _enabled = false;
                }
            }
            else
            {
                _logger.LogInformation("Redis cache is disabled in configuration.");
            }
        }

        public async Task<T> GetAsync<T>(string key)
        {
            if (!_enabled) return default;

            try
            {
                var value = await _db.StringGetAsync(key);
                if (value.IsNullOrEmpty)
                {
                    return default;
                }

                return JsonConvert.DeserializeObject<T>(value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving value from Redis cache for key {Key}", key);
                return default;
            }
        }

        public async Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            if (!_enabled) return false;

            try
            {
                var serializedValue = JsonConvert.SerializeObject(value);
                return await _db.StringSetAsync(key, serializedValue, expiry ?? _defaultExpiry);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting value in Redis cache for key {Key}", key);
                return false;
            }
        }

        public async Task<bool> RemoveAsync(string key)
        {
            if (!_enabled) return false;

            try
            {
                return await _db.KeyDeleteAsync(key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing key {Key} from Redis cache", key);
                return false;
            }
        }

        public async Task<bool> ExistsAsync(string key)
        {
            if (!_enabled) return false;

            try
            {
                return await _db.KeyExistsAsync(key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if key {Key} exists in Redis cache", key);
                return false;
            }
        }

        public async Task<bool> RemoveByPatternAsync(string pattern)
        {
            if (!_enabled) return false;

            try
            {
                // This is a more complex operation in Redis that requires scripting
                var endpoints = _redis.GetEndPoints();
                var success = true;

                foreach (var endpoint in endpoints)
                {
                    var server = _redis.GetServer(endpoint);
                    var keys = server.Keys(pattern: $"{pattern}");

                    foreach (var key in keys)
                    {
                        if (!await _db.KeyDeleteAsync(key))
                        {
                            success = false;
                        }
                    }
                }

                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing keys matching pattern {Pattern} from Redis cache", pattern);
                return false;
            }
        }
    }
}