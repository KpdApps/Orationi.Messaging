namespace KpdApps.Orationi.Messaging.ServerCore.ProcessHosts
{
    public interface IProcessHost
    {
        int RequestCode { get; }

        bool IsSynchronous { get; }

        string CloseReason { get; }

        string QueueCode { get; }

        void Run();
    }
}
