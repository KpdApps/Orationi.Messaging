using System;
using System.Collections.Generic;
using System.Text;

namespace KpdApps.Orationi.Messaging.ServerConsole.Pipeline
{
    internal class PipelineStepDescription
    {
        public byte[] Assembly { get; set; }

        public string Class { get; set; }

        public int Order { get; set; }

        public bool IsAsynchronous { get; set; }
    }
}
