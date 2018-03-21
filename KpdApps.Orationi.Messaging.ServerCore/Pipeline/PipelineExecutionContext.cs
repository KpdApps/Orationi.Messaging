using KpdApps.Orationi.Messaging.Sdk;
using KpdApps.Orationi.Messaging.Sdk.Plugins;
using KpdApps.Orationi.Messaging.ServerCore.Workflow;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace KpdApps.Orationi.Messaging.ServerCore.Pipeline
{
    internal class PipelineExecutionContext : IPipelineExecutionContext
    {
        public string RequestBody { get; set; }

        public string ResponseBody { get; set; }

        public string ResponseUser { get; set; }

        public string ResponseSystem { get; set; }

        public int? StatusCode { get; set; }

        public IDictionary PipelineValues { get; }

        public IDictionary PluginStepSettings { get; set; }

        public IWorkflowExecutionContext WorkflowExecutionContext { get; set; }

        internal PipelineExecutionContext()
        {
            PipelineValues = new Dictionary<string, object>();
            PluginStepSettings = new Dictionary<string, object>();
        }
    }
}
