using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KpdApps.Orationi.Messaging.DataAccess.Models
{
    public class RegisteredPlugin
    {
        public Guid Id { get; set; }

        [ForeignKey("PluginAssembly")]
        public Guid AssemblyId { get; set; }

        public string Class { get; set; }

        public virtual PluginAssembly PluginAssembly { get; set; }
        public virtual List<PluginActionSetItem> PluginActionSetItems { get; set; }
    }
}
