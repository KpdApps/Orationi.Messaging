namespace KpdApps.Orationi.Messaging.Sdk.Plugins
{
    public interface IPipelinePlugin
    {
        IExecuteContext Context { get; }

        string RequestContractUri { get; }

        string ResponseContractUri { get; }

        string[] GlobalSettingsList { get; }

        string[] LocalSettingsList { get; }

        void BeforeExecution();

        void Execute();

        void AfterExecution();
    }
}
