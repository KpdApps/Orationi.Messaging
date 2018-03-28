using System;

namespace KpdApps.Orationi.Messaging.DataAccess.Common.Models
{
    public class PluginActionSetItem
    {
        public Guid Id { get; set; }

        public int RequestCode { get; set; }

        public Guid RegisteredPluginId { get; set; }

        public Guid PluginActionSetId { get; set; }

        public int Order { get; set; }

        public bool IsAsynchronous { get; set; }

        public string Configuration { get; set; }
    }
}
