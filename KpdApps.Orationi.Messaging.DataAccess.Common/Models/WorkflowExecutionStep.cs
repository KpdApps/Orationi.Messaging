using System;

namespace KpdApps.Orationi.Messaging.DataAccess.Common.Models
{
    public class WorkflowExecutionStep
    {
        public Guid Id { get; set; }

        public Guid WorkflowId { get; set; }

        public Guid PluginActionSetId { get; set; }

        public int StatusCode { get; set; }

        public string RequestBody { get; set; }

        public string ResponseBody { get; set; }

        public string ExecutionVariables { get; set; }
    }
}
