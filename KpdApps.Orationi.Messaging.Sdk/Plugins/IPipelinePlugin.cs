namespace KpdApps.Orationi.Messaging.Sdk.Plugins
{
    public interface IPipelinePlugin
    {
        IExecuteContext Context { get; }

        string RequestContractUri { get; }

        string ResponseContractUri { get; }

        void BeforeExecution();

        void Execute();

        void AfterExecution();
    }
}
