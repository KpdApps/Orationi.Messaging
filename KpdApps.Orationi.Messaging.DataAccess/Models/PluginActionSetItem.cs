using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KpdApps.Orationi.Messaging.DataAccess.Models
{
    public class PluginActionSetItem
    {
        public PluginActionSetItem()
        {
            WorkflowExecutionSteps = new List<WorkflowExecutionStep>();
        }

        [Key]
        public Guid Id { get; set; }

        [ForeignKey("RegisteredPlugin")]
        public Guid RegisteredPluginId { get; set; }

        [ForeignKey("PluginActionSet")]
        public Guid PluginActionSetId { get; set; }

        public int Order { get; set; }

        public string Configuration { get; set; }

        public virtual RegisteredPlugin RegisteredPlugin { get; set; }

        public virtual PluginActionSet PluginActionSet { get; set; }

        public virtual List<WorkflowExecutionStep> WorkflowExecutionSteps { get; set; }
    }
}
