using KpdApps.Orationi.Messaging.ServerCore.PluginHosts;
using System;

namespace KpdApps.Orationi.Messaging.ServerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //TODO: Проверить как себя ведет Env для обычного .NET Framework
            //Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));

            ProcessHostManager phm = new ProcessHostManager("localhost", "orationi", "orationi");

            phm.Add(60105, false);
            Console.WriteLine(" Press [enter] to stop.");
            Console.ReadLine();

            //TODO: разобраться почему после нажатия Enter не происходит завершения работы хост менеджера
            phm.Remove(60105, false);
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
