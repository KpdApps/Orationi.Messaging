using System;

namespace KpdApps.Orationi.Messaging.ServerCore.Workflow
{
    public class WorkflowAction
    {
        public Guid WorkflowId { get; set; }

        public Guid PluginActionSetId { get; set; }

        public int Order { get; set; }
    }
}
