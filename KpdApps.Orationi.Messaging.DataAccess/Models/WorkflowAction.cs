using System;
using System.Collections.Generic;
using System.Text;

namespace KpdApps.Orationi.Messaging.DataAccess.Models
{
    public class WorkflowAction
    {
        public Guid Id { get; set; }

        public Guid WorkflowId { get; set; }

        public Workflow Workflow { get; set; }

        public Guid PluginActionSetId { get; set; }

        public PluginActionSet PluginActionSet { get; set; }

        public string Description { get; set; }

        public int Order { get; set; }
    }
}
