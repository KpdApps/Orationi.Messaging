using System;
using System.Collections.Generic;
using System.Text;

namespace KpdApps.Orationi.Messaging.ServerCore.ProcessHosts
{
    public static class ProcessHostFactory
    {
        public static IProcessHost GetPluginHost(string hostname, string username, string password, int requestCode, bool isSynchronous)
        {
            if (isSynchronous)
            {
                return new SynchronousProcessHost(hostname, username, password, requestCode);
            }
            else
            {
                throw new NotImplementedException();
                //return new AsynchronousPluginHost(hostname, username, password, requestCode);
            }
        }
    }
}
