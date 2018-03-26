using System.ServiceProcess;
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
