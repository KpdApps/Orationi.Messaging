namespace KpdApps.Orationi.Messaging.ServerCore.PluginHosts
{
    public interface IPluginHost
    {
        int RequestCode { get; }

        bool IsSynchronous { get; }

        string CloseReason { get; }

        string QueueCode { get; }

        void Run();
    }
}
