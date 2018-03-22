using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace KpdApps.Orationi.WinNTHostService.Host
{
    public static class ServiceHostExtension
    {
        public static void RunAsServiceHost(this IWebHost webHost)
        {
            var host = new ServiceHost(webHost);
            ServiceBase.Run(host);
        }
    }
}
