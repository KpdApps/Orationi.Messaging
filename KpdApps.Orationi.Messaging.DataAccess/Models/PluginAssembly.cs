﻿using System;

namespace KpdApps.Orationi.Messaging.DataAccess.Models
{
    public class PluginAssembly
    {
        public Guid Id { get; set; }

        public byte[] Assembly { get; set; }

        public DateTime ModifiedOn { get; set; }
    }
}
