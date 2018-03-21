using System;
using System.Collections;

namespace KpdApps.Orationi.Messaging.Sdk
{
    public interface IPipelineExecutionContext
    {
        string RequestBody { get; set; }

        string ResponseBody { get; set; }

        string ResponseUser { get; set; }

        string ResponseSystem { get; set; }

	    Nullable<int> StatusCode { get; set; }

        IDictionary PipelineValues { get; }

        IDictionary PluginStepSettings { get; set; }

        IWorkflowExecutionContext WorkflowExecutionContext { get; set; }
    }
}