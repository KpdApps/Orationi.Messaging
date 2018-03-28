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

            ProcessHostManager phm = new ProcessHostManager("vm-co-crmt-01.exiar.ru", "orationi", "orationi");
            phm.Add(60105, false);
            //phm.Add(1, false);

            Console.WriteLine(" Press [enter] to stop.");
            Console.ReadLine();
            phm.Remove(60105, false);
            //phm.Remove(1, false);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
