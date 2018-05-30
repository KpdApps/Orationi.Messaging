using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KpdApps.Orationi.Messaging.DataAccess;

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
