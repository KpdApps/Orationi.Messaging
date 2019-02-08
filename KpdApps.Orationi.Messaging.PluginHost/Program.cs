using System.ServiceProcess;

namespace KpdApps.Orationi.Messaging.PluginHost
{
    static class Program
    {
        static void Main()
        {
            var servicesToRun = new ServiceBase[]
            {
                new PluginHost()
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}
