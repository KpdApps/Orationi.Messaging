using System;
using System.Collections.Generic;
using System.Text;

namespace KpdApps.Orationi.Messaging.DataAccess.Models
{
    public class PipelineStep
    {
        public Guid Id { get; set; }

        public Guid AssemblyId { get; set; }

        public string Class { get; set; }
    }
}
