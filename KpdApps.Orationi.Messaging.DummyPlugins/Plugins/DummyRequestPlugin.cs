using KpdApps.Orationi.Messaging.Sdk;
using KpdApps.Orationi.Messaging.Sdk.Plugins;

namespace KpdApps.Orationi.Messaging.DummyPlugins.Plugins
{
    public class DummyRequestPlugin : BasePipelinePlugin, IPipelinePlugin
    {
        public DummyRequestPlugin(IPipelineExecutionContext context)
            : base(context)
        {
            int dummyValue = (int)Context.PipelineValues["DummyValue"]; //.Add("DummyValue", 1);
            dummyValue++;
            Context.PipelineValues["DummyValue"] = dummyValue;
        }
    }
}
