using System;
using System.Collections.Generic;
using System.Text;

namespace KpdApps.Orationi.Messaging.ServerCore.PluginHosts
{
    public static class PluginHostsFactory
    {
        public static IPluginHost GetPluginHost(string hostname, string username, string password, int requestCode, bool isSynchronous)
        {
            if (isSynchronous)
            {
                return new SynchronousPluginHost(hostname, username, password, requestCode);
            }
            else
            {
                return new AsynchronousPluginHost(hostname, username, password, requestCode);
            }
        }
    }
}
