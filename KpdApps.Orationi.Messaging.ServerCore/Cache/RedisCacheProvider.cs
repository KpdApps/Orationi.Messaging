﻿using System;
using KpdApps.Orationi.Messaging.Sdk.Cache;

namespace KpdApps.Orationi.Messaging.ServerCore.Cache
{
    class RedisCacheProvider : ICacheProvider
    {
        public string GetValue(string key)
        {
            throw new NotImplementedException();
        }

        public string TryGetValue(string key)
        {
            throw new NotImplementedException();
        }

        public void SetValue(string key, string value, int expirePeriod)
        {
            throw new NotImplementedException();
        }
    }
}
