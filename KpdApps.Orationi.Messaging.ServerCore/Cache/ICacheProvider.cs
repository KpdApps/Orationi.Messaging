namespace KpdApps.Orationi.Messaging.ServerCore.Cache
{
    public interface ICacheProvider
    {
        string GetValue(string key);

        string TryGetValue(string key);

        void SetValue(string key, string value);
    }
}
