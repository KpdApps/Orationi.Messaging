using KpdApps.Orationi.Messaging.Sdk;
using KpdApps.Orationi.Messaging.Sdk.Plugins;

namespace KpdApps.Orationi.Messaging.DummyPlugins.Plugins
{
    public class FinishDummyPlugin : BasePipelinePlugin, IPipelinePlugin
    {
        const string SystemName = "DummySystem";

        const string FakeUserName = "Dummy";

        public FinishDummyPlugin(IPipelineExecutionContext context)
            : base(context)
        {

        }

        public override void Execute()
        {
            DummyResponse response = new DummyResponse
            {
                MessageId = Context.WorkflowExecutionContext.MessageId.ToString(),
                RequestCode = Context.WorkflowExecutionContext.RequestCode,
                Status = Context.PipelineValues["DummyValue"].ToString()
            };
            Context.ResponseBody = response.Serialize();
        }

        public override void AfterExecution()
        {
            base.AfterExecution();
            Context.ResponseSystem = SystemName;
            Context.ResponseUser = FakeUserName;
        }
    }
}
