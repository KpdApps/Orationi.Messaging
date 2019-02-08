using System.ServiceProcess;

namespace KpdApps.Orationi.Messaging.CallbackHost
{
    static class Program
    {
        static void Main()
        {
            var servicesToRun = new ServiceBase[]
            {
                new CallbackHost()
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}
