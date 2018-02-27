using KpdApps.Orationi.Messaging.ServerCore;
using KpdApps.Orationi.Messaging.ServerCore.PluginHosts;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace KpdApps.Orationi.Messaging.ServerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));

            PluginsHostManager phm = new PluginsHostManager("localhost", "orationi", "orationi");
            phm.Add(1, true);
            phm.Add(1, false);

            Console.WriteLine(" Press [enter] to stop.");
            Console.ReadLine();
            phm.Remove(1, true);
            phm.Remove(1, false);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
