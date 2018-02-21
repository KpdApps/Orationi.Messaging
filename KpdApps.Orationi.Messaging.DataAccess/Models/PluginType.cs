using System;

namespace KpdApps.Orationi.Messaging.DataAccess.Models
{
    public class PluginType
    {
        public Guid Id { get; set; }

        public Guid AssemblyId { get; set; }

        public string Class { get; set; }
    }
}
