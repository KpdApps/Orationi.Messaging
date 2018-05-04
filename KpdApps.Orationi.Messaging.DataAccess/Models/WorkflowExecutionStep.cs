using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KpdApps.Orationi.Messaging.DataAccess.Models
{
    public class WorkflowExecutionStep
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("Workflow")]
        public Guid WorkflowId { get; set; }

        [ForeignKey("PluginActionSet")]
        public Guid PluginActionSetId { get; set; }

        public int StatusCode { get; set; }

        public string RequestBody { get; set; }

        public string ResponseBody { get; set; }

        public string ExecutionVariables { get; set; }

        [ForeignKey("Message")]
        public Nullable<Guid> MessageId { get; set; }

        public virtual Workflow Workflow { get; set; }

        public virtual PluginActionSet PluginActionSet { get; set; }

        public virtual WorkflowExecutionStepsStatusCode WorkflowExecutionStepsStatusCode { get; set; }

        public virtual Message Message { get; set; }
    }
}
