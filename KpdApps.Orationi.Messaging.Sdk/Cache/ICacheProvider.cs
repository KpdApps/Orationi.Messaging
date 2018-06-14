namespace KpdApps.Orationi.Messaging.Sdk.Cache
{
    public interface ICacheProvider
    {
        string GetValue(string key);

        string TryGetValue(string key);

        void SetValue(string key, string value, int expirePeriod);
    }
}
