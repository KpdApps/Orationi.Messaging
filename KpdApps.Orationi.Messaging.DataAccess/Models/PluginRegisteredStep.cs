using System;

namespace KpdApps.Orationi.Messaging.DataAccess.Models
{
    public class PluginRegisteredStep
    {
        public Guid Id { get; set; }

        public int RequestCode { get; set; }

        public Guid PluginTypeId { get; set; }

        public int Order { get; set; }

        public bool IsAsynchronous { get; set; }

        public string Configuration { get; set; }
    }
}
