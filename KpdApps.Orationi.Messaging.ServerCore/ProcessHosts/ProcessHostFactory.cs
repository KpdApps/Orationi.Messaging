namespace KpdApps.Orationi.Messaging.ServerCore.ProcessHosts
{
    public static class ProcessHostFactory
    {
        public static IProcessHost GetPluginHost(string hostname, string username, string password, int requestCode, bool isSynchronous)
        {
            if (isSynchronous)
            {
                return new SynchronousProcessHost(hostname, username, password, requestCode);
            }

            return new AsynchronousProcessHost(hostname, username, password, requestCode);
        }
    }
}
