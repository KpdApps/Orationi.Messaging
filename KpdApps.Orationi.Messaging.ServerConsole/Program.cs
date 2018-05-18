using KpdApps.Orationi.Messaging.DataAccess;
using KpdApps.Orationi.Messaging.ServerCore.PluginHosts;
using System;
using System.Collections.Generic;

namespace KpdApps.Orationi.Messaging.ServerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            ProcessHostManager processHostManager = new ProcessHostManager("localhost", "orationi", "orationi");
			//List<(int RequestCode, bool IsSync)> plugins = plugins = new List<(int RequestCode, bool IsSync)>();

			//using (var dbContext = new OrationiDatabaseContext())
			//{
			//    foreach (var requestCode in dbContext.RequestCodes)
			//    {
			//        plugins.AddRange(new[] { (requestCode.Id, true), (requestCode.Id, false) });
			//    }
			//}

			//plugins.ForEach(p => processHostManager.Add(p.RequestCode, p.IsSync));
			processHostManager.Add(60109, false);

			Console.WriteLine(" Press [enter] to stop.");
            Console.ReadLine();

			//TODO: разобраться почему после нажатия Enter не происходит завершения работы хост менеджера
			//plugins.ForEach(p => processHostManager.Remove(p.RequestCode, p.IsSync));
			processHostManager.Remove(60109, false);

			Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
