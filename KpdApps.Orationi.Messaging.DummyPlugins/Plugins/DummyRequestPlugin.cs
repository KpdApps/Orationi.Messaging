using KpdApps.Orationi.Messaging.Sdk;
using KpdApps.Orationi.Messaging.Sdk.Plugins;

namespace KpdApps.Orationi.Messaging.DummyPlugins.Plugins
{
    public class DummyRequestPlugin : BasePipelinePlugin, IPipelinePlugin
    {
        public DummyRequestPlugin(IPipelineExecutionContext context)
            : base(context)
        {
            
        }

        public override void Execute()
        {
            int dummyValue = (int)Context.PipelineValues["DummyValue"];
            dummyValue++;
            Context.PipelineValues["DummyValue"] = dummyValue;
        }
    }
}
