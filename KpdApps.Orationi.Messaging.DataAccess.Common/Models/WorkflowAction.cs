using System;

namespace KpdApps.Orationi.Messaging.DataAccess.Common.Models
{
    public class WorkflowAction
    {
        public Guid Id { get; set; }

        public Guid WorkflowId { get; set; }

        public Guid PluginActionSetId { get; set; }

        public string Description { get; set; }

        public int Order { get; set; }
    }
}
