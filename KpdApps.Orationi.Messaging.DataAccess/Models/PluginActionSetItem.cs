using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace KpdApps.Orationi.Messaging.DataAccess.Models
{
    public class PluginActionSetItem
    {
        public Guid Id { get; set; }

        public int RequestCode { get; set; }

        [ForeignKey("RegisteredPlugin")]
        public Guid RegisteredPluginId { get; set; }

        public Guid PluginActionSetId { get; set; }

        public int Order { get; set; }

        public bool IsAsynchronous { get; set; }

        public string Configuration { get; set; }

        public virtual RegisteredPlugin RegisteredPlugin { get; set; }

        public virtual PluginActionSet PluginActionSet { get; set; }
    }
}
