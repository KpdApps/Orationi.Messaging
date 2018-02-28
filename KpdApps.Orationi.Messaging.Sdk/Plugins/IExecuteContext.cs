using System;
using System.Collections;
using System.Collections.ObjectModel;

namespace KpdApps.Orationi.Messaging.Sdk.Plugins
{
    public interface IExecuteContext
    {
        string RequestBody { get; }

        string ResponseBody { get; set; }

        string ResponseUser { get; set; }

        string ResponseSystem { get; set; }

        Nullable<int> StatusCode { get; set; }

        IDictionary PipelineValues { get; }

        IDictionary GlobalSettings { get; set; }

        IDictionary PluginStepSettings { get; set; }
    }
}