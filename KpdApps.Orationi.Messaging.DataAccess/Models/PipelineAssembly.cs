using System;
using System.Collections.Generic;
using System.Text;

namespace KpdApps.Orationi.Messaging.DataAccess.Models
{
    public class PipelineAssembly
    {
        public Guid Id { get; set; }

        public byte[] Assembly { get; set; }
    }
}
