using System;
using System.Collections.Generic;
using System.Text;

namespace KpdApps.Orationi.Messaging.ServerCore.Pipeline
{
    internal class PipelineStepDescription
    {
        public Guid AssemblyId { get; set; }

        public string Class { get; set; }

        public int Order { get; set; }

        public DateTime Modified { get; set; }

        public bool IsAsynchronous { get; set; }

        public string ConfigurationString { get; set; }
    }
}
