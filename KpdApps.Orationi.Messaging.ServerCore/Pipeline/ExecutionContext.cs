using KpdApps.Orationi.Messaging.Sdk.Plugins;
using System.Collections;
using System.Collections.Generic;

namespace KpdApps.Orationi.Messaging.ServerCore.Pipeline
{
    internal class ExecuteContext : IExecuteContext
    {
        public string RequestBody { get; internal set; }

        public string ResponseBody { get; set; }

        public IDictionary PipelineValues { get; }

        public IDictionary GlobalSettings { get; set; }

        public IDictionary PluginStepSettings { get; set; }

        internal ExecuteContext()
        {
            PipelineValues = new Dictionary<string, object>();
        }
    }
}
