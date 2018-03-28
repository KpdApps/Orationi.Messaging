using System;
using System.Collections.Generic;

namespace KpdApps.Orationi.Messaging.DataAccess.Common.Models
{
    public class PluginAssembly
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public byte[] Assembly { get; set; }

        public DateTime Modified { get; set; }

        public virtual List<RegisteredPlugin> RegisteredPlugins { get; set; }

        public virtual List<PluginActionSetItem> PluginActionSetItems { get; set; }
    }
}
