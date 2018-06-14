using System;
using System.Linq;
using KpdApps.Orationi.Messaging.DataAccess;
using KpdApps.Orationi.Messaging.DataAccess.Models;
using KpdApps.Orationi.Messaging.Sdk.Cache;

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
                .CacheRequestResponse
                .FirstOrDefault(c => c.Key == key)
                ?.Value;

            return value;
        }

        /// <summary>
        /// Сохранение значения по ключу в кэш
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="value">Значение</param>
        /// <param name="expirePeriod">Период актуальности (дни)</param>
        public void SetValue(string key, string value, int expirePeriod)
        {
            CacheRequestResponse cacheEntity = _dbContext
                .CacheRequestResponse
                .FirstOrDefault(c => c.Key == key);

            if (cacheEntity is null)
            {
                cacheEntity = new CacheRequestResponse
                {
                    Key = key,
                    Value = value,
                    ExpireDate = DateTime.Now.AddDays(expirePeriod)
                };
                _dbContext.CacheRequestResponse.Add(cacheEntity);
            }
            else
            {
                cacheEntity.Value = value;
            }

            _dbContext.SaveChanges();
        }
    }
}