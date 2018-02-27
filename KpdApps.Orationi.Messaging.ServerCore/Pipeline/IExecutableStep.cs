namespace KpdApps.Orationi.Messaging.ServerCore.Pipeline
{
    public interface IExecutableStep
    {
        bool IsAsynchronous { get; }
        int Order { get; }
        void Execute();
    }
}
