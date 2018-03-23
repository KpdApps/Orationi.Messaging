using KpdApps.Orationi.Messaging.Sdk;
using KpdApps.Orationi.Messaging.Sdk.Plugins;
using KpdApps.Orationi.Messaging.ServerCore.Workflow;
using System;
using System.Collections;
using System.Collections.Generic;

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

        public Guid MessageId => _workflowExecutionContext.MessageId;

        public int RequestCode => _workflowExecutionContext.RequestCode;

        public string MessageBody => _workflowExecutionContext.MessageBody;

        public IDictionary GlobalSettings => _workflowExecutionContext.GlobalSettings;

        private IWorkflowExecutionContext _workflowExecutionContext;

        internal PipelineExecutionContext(IWorkflowExecutionContext workflowExecutionContext)
        {
            PipelineValues = new Dictionary<string, object>();
            PluginStepSettings = new Dictionary<string, object>();
            _workflowExecutionContext = workflowExecutionContext;
        }
    }
}
