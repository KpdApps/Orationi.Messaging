using System;
using System.Collections.Generic;
using System.Text;

namespace KpdApps.Orationi.Messaging.ServerCore.PluginHosts
{
    public class ManagedPlugin
    {
        public int RequestCode { get; set; }

        public bool IsSynchronous { get; set; }

        public string QueueCode => $"queue-{RequestCode}-{Convert.ToInt32(IsSynchronous)}";
    }
}
