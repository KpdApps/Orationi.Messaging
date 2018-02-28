using KpdApps.Orationi.Messaging.Sdk.Plugins;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace KpdApps.Orationi.Messaging.ServerCore.Pipeline
{
    internal class ExecuteContext : IExecuteContext
    {
        public string RequestBody { get; internal set; }

        public string ResponseBody { get; set; }

        public string ResponseUser { get; set; }

        public string ResponseSystem { get; set; }

        public int? StatusCode { get; set; }

        public IDictionary PipelineValues { get; }

        public IDictionary GlobalSettings { get; set; }

        public IDictionary PluginStepSettings { get; set; }

        internal ExecuteContext()
        {
            PipelineValues = new Dictionary<string, object>();
            GlobalSettings = new Dictionary<string, string>();
            PluginStepSettings = new Dictionary<string, object>();
        }
    }
}
