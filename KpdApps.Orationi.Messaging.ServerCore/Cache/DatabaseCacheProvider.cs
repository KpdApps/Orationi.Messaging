using System;
using System.Linq;
using KpdApps.Orationi.Messaging.DataAccess;
using KpdApps.Orationi.Messaging.DataAccess.Models;

namespace KpdApps.Orationi.Messaging.ServerCore.Cache
{
    public class DatabaseCacheProvider : ICacheProvider
    {
        private readonly OrationiDatabaseContext _dbContext;

        public DatabaseCacheProvider(OrationiDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public string GetValue(string key)
        {
            var value = TryGetValue(key);

            if (value is null)
            {
                throw new InvalidOperationException($"Ключ ({key}) отсутствует в базе данных");
            }

            return value;
        }

        public string TryGetValue(string key)
        {
            var value = _dbContext
                .CacheSparkRequests
                .FirstOrDefault(c => c.Key == key.ToLower())
                ?.Value;

            return value;
        }

        public void SetValue(string key, string value)
        {
            CacheSparkRequest cacheEntity = _dbContext
                .CacheSparkRequests
                .FirstOrDefault(c => c.Key == key.ToLower());

            if (cacheEntity is null)
            {
                cacheEntity = new CacheSparkRequest
                {
                    Key = key.ToLower(),
                    Value = value.ToLower(),
                    Created = DateTime.Now
                };
                _dbContext.CacheSparkRequests.Add(cacheEntity);
            }
            else
            {
                cacheEntity.Value = value.ToLower();
            }

            _dbContext.SaveChanges();
        }
    }
}