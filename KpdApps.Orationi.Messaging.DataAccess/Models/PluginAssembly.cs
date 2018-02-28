using System;

namespace KpdApps.Orationi.Messaging.DataAccess.Models
{
    public class PluginAssembly
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public byte[] Assembly { get; set; }

        public DateTime Modified { get; set; }
    }
}
