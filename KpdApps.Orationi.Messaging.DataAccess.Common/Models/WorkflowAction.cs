using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace KpdApps.Orationi.Messaging.DataAccess.Common.Models
{
    public class WorkflowAction
    {
        public Guid Id { get; set; }

        [ForeignKey("Workflow")]
        public Guid WorkflowId { get; set; }

        [ForeignKey("PluginActionSet")]
        public Guid PluginActionSetId { get; set; }

        public string Description { get; set; }

        public int Order { get; set; }

        public virtual PluginActionSet PluginActionSet { get; set; }

        public virtual Workflow Workflow { get; set; }
    }
}
