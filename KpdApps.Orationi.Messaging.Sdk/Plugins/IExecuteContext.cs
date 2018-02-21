using System.Collections;

namespace KpdApps.Orationi.Messaging.Sdk.Plugins
{
    public interface IExecuteContext
    {
        string RequestBody { get; }

        string ResponseBody { get; set; }

        IDictionary PipelineValues { get; }
    }
}