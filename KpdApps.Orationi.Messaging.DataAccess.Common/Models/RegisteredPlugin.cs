using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace KpdApps.Orationi.Messaging.DataAccess.Common.Models
{
    public class RegisteredPlugin
    {
        public Guid Id { get; set; }

        [ForeignKey("PluginAssembly")]
        public Guid AssemblyId { get; set; }

        public string Class { get; set; }

        public virtual PluginAssembly PluginAssembly { get; set; }
    }
}
