
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace KpdApps.Orationi.Messaging.Sdk.Plugins
{
    public abstract class BasePipelinePlugin : IPipelinePlugin
    {
        public IExecuteContext Context { get; set; }

        public virtual string RequestContractUri { get; protected set; }

        public virtual string ResponseContractUri { get; protected set; }

        public BasePipelinePlugin(IExecuteContext context)
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
