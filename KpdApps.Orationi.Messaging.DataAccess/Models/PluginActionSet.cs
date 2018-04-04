using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KpdApps.Orationi.Messaging.DataAccess.Models
{
    public class PluginActionSet
    {
        public PluginActionSet()
        {
            WorkflowExecutionSteps = new List<WorkflowExecutionStep>();
            PluginActionSetItems = new List<PluginActionSetItem>();
            WorkflowActions = new List<WorkflowAction>();
        }

        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public virtual List<WorkflowExecutionStep> WorkflowExecutionSteps { get; set; }

        public virtual List<PluginActionSetItem> PluginActionSetItems { get; set; }

        public virtual List<WorkflowAction> WorkflowActions { get; set; }
    }
}
