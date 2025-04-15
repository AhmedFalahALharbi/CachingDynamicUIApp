using StackExchange.Redis;
using System;

namespace CachingDynamicUIApp.Services
{
    public class RedisService
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _db;

        public RedisService(string connectionString)
        {
            _redis = ConnectionMultiplexer.Connect(connectionString);
            _db = _redis.GetDatabase();
        }

        public bool StringSet(string key, string value, TimeSpan? expiry = null)
        {
            return _db.StringSet(key, value, expiry);
        }

        public string StringGet(string key)
        {
            return _db.StringGet(key);
        }

        public bool KeyExists(string key)
        {
            return _db.KeyExists(key);
        }

        public bool KeyDelete(string key)
        {
            return _db.KeyDelete(key);
        }
    }
}