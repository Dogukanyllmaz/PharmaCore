using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Core.CrossCuttingConcerns.Caching.Redis
{
    public class RedisCacheManager : ICacheManager
    {
        private readonly IDatabase _database;
        private readonly IConnectionMultiplexer _connection;

        public RedisCacheManager()
        {
            _connection = ServiceTool.ServiceProvider.GetService<IConnectionMultiplexer>();
            _database = _connection.GetDatabase();
        }

        public void Add(string key, object value, int duration)
        {
            var json = JsonSerializer.Serialize(value);
            _database.StringSet(key, json, TimeSpan.FromMinutes(duration));
        }

        public T Get<T>(string key)
        {
            var value = _database.StringGet(key);
            if (!value.HasValue)
            {
                return default;
            }
            return JsonSerializer.Deserialize<T>(value);
        }

        public object Get(string key)
        {
            var value = _database.StringGet(key);

            if (!value.HasValue)
            {
                return null;
            }
            return JsonSerializer.Deserialize<object>(value);
        }

        public bool isAdd(string key)
        {
            return _database.KeyExists(key);
        }

        public void Remove(string key)
        {
            _database.KeyDelete(key);
        }

        public void RemoveByPattern(string pattern)
        {
            var endpoints = _connection.GetEndPoints();
            foreach (var endpoint in endpoints)
            {
                var server = _connection.GetServer(endpoint);
                var keys = server.Keys(pattern: $"{pattern}*").ToList();
                foreach (var key in keys)
                {
                    _database.KeyDelete(key);
                }
            }
        }
    }
}
