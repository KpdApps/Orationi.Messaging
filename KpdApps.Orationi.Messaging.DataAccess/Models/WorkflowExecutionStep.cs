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

        [ForeignKey("PluginActionSetItem")]
        public Guid PluginActionSetItemId { get; set; }

        public int StatusCode { get; set; }

        public string RequestBody { get; set; }

        public string ResponseBody { get; set; }

        public DateTime Created { get; set; }

        public string PipelineValues { get; set; }

        [ForeignKey("Message")]
        public Nullable<Guid> MessageId { get; set; }

        public virtual Workflow Workflow { get; set; }

        public virtual PluginActionSetItem PluginActionSetItem { get; set; }

        public virtual WorkflowExecutionStepsStatusCode WorkflowExecutionStepsStatusCode { get; set; }

        public virtual Message Message { get; set; }
    }
}
