using System.Reflection;
using KpdApps.Orationi.Messaging.Sdk.Attributes;

namespace KpdApps.Orationi.Messaging.Sdk.Plugins
{
    public abstract class BasePipelinePlugin : IPipelinePlugin
    {
        public IPipelineExecutionContext Context { get; set; }

        public virtual string RequestContractInUri { get; }

        public virtual string RequestContractOutUri { get; }

        public virtual string ResponseContractInUri { get; }

        public virtual string ResponseContractOutUri { get; }

        public virtual string[] GlobalSettingsList => new string[] { };

        public virtual string[] LocalSettingsList => new string[] { };

        protected BasePipelinePlugin(IPipelineExecutionContext context)
        {
            Context = context;
            RequestContractInUri = ((ContractAttribute)GetType().GetCustomAttribute(typeof(RequestContractInAttribute)))?.Uri;
            RequestContractOutUri = ((ContractAttribute)GetType().GetCustomAttribute(typeof(RequestContractOutAttribute)))?.Uri;
            ResponseContractInUri = ((ContractAttribute)GetType().GetCustomAttribute(typeof(ResponseContractInAttribute)))?.Uri;
            ResponseContractOutUri = ((ContractAttribute)GetType().GetCustomAttribute(typeof(ResponseContractOutAttribute)))?.Uri;
        }

        public virtual void AfterExecution()
        {
            if (!string.IsNullOrEmpty(ResponseContractOutUri) && !string.IsNullOrEmpty(Context.ResponseBody))
            {
                XsdValidator.ValidateXml(Context.ResponseBody, new[] { ResponseContractOutUri }, this.GetType());
            }
        }

        public virtual void BeforeExecution()
        {
            if (!string.IsNullOrEmpty(RequestContractInUri) && !string.IsNullOrEmpty(Context.RequestBody))
            {
                XsdValidator.ValidateXml(Context.RequestBody, new[] { RequestContractInUri }, this.GetType());
            }
        }

        public virtual void Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}
