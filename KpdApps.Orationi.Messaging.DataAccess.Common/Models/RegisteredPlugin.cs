using System;

namespace KpdApps.Orationi.Messaging.DataAccess.Common.Models
{
    public class RegisteredPlugin
    {
        public Guid Id { get; set; }

        public Guid AssemblyId { get; set; }

        public string Class { get; set; }
    }
}
