using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KpdApps.Orationi.Messaging.DataAccess.Models
{
    public class PluginAssembly
    {
        public PluginAssembly()
        {
            RegisteredPlugins = new List<RegisteredPlugin>();
        }

        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public byte[] Assembly { get; set; }

        public DateTime Modified { get; set; }

        public virtual List<RegisteredPlugin> RegisteredPlugins { get; set; }
    }
}
