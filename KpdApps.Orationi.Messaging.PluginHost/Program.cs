using System.ServiceProcess;

namespace KpdApps.Orationi.Messaging.PluginHost
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new PluginHost(),
                new CallbackHost()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
