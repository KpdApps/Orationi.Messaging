namespace KpdApps.Orationi.Messaging.Sdk.Plugins
{
    public abstract class BasePipelinePlugin : IPipelinePlugin
    {
        public IPipelineExecutionContext Context { get; set; }

        public virtual string MessageContractUri { get; protected set; }

        public virtual string RequestContractUri { get; protected set; }

        public virtual string ResponseContractUri { get; protected set; }

        public virtual string[] GlobalSettingsList => new string[] { };

        public virtual string[] LocalSettingsList => new string[] { };

        public BasePipelinePlugin(IPipelineExecutionContext context)
        {
            Context = context;
        }

        public virtual void AfterExecution()
        {
            if (!string.IsNullOrEmpty(ResponseContractUri) && !string.IsNullOrEmpty(Context.ResponseBody))
            {
                XsdValidator.ValidateXml(Context.ResponseBody, new[] { ResponseContractUri }, this.GetType());
            }
        }

        public virtual void BeforeExecution()
        {
            if (!string.IsNullOrEmpty(RequestContractUri) && !string.IsNullOrEmpty(Context.RequestBody))
            {
                XsdValidator.ValidateXml(Context.RequestBody, new[] { RequestContractUri }, this.GetType());
            }
        }

        public virtual void Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}
