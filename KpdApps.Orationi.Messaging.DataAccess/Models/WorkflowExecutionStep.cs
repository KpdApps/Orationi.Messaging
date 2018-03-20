using System;
using System.Collections.Generic;
using System.Text;

namespace KpdApps.Orationi.Messaging.DataAccess.Models
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
