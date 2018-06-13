namespace KpdApps.Orationi.Messaging.Sdk.Plugins
{
    public interface IPipelinePlugin
    {
        /// <summary>
        /// Current plugins pipeline execution context.
        /// </summary>
        IPipelineExecutionContext Context { get; }

        /// <summary>
        /// Uri to request contract uri in plugin-assembly.
        /// </summary>
        string RequestContractInUri { get; }

        string RequestContractOutUri { get; }

        string ResponseContractInUri { get; }

        /// <summary>
        /// Uri to response contract uri in plugin-assembly.
        /// </summary>
        string ResponseContractOutUri { get; }

        /// <summary>
        /// List of used Global Settings.
        /// Should be validate on plugin registration action and before execution.
        /// </summary>
        string[] GlobalSettingsList { get; }

        /// <summary>
        /// List of used Local Settings.
        /// Should be validate on plugin registration action.
        /// </summary>
        string[] LocalSettingsList { get; }

        void BeforeExecution();

        void Execute();

        void AfterExecution();
    }
}
