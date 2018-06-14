using KpdApps.Orationi.Messaging.DataAccess;
using KpdApps.Orationi.Messaging.Sdk.Cache;

namespace KpdApps.Orationi.Messaging.ServerCore.Cache
{
    public class CacheProviderFactory
    {
        public static ICacheProvider Create(OrationiDatabaseContext dbContext)
        {
            return new DatabaseCacheProvider(dbContext);
        }
    }
}
