using KpdApps.Orationi.Messaging.Sdk;
using KpdApps.Orationi.Messaging.Sdk.Plugins;

namespace KpdApps.Orationi.Messaging.DummyPlugins.Plugins
{
    public class FinishDummyPlugin : BasePipelinePlugin, IPipelinePlugin
    {
        public FinishDummyPlugin(IPipelineExecutionContext context)
            : base(context)
        {
            DummyResponse response = new DummyResponse
            {
                MessageId = context.WorkflowExecutionContext.MessageId.ToString(),
                RequestCode = context.WorkflowExecutionContext.RequestCode,
                Status = Context.PipelineValues["DummyValue"].ToString()
            };
            Context.ResponseBody = response.Serialize();
        }
    }
}
