using System;
using System.Collections.Generic;
using KpdApps.Orationi.Messaging.DataAccess;
using KpdApps.Orationi.Messaging.ServerCore.Callback;
using KpdApps.Orationi.Messaging.ServerCore.ProcessHosts;

namespace KpdApps.Orationi.Messaging.ServerConsole
{
    class Program
    {
        private static readonly bool ShouldProcessSingleRequestCode = true;
        private static readonly int SingleProcessingRequestCode = 60109;
        private static readonly bool IsSync = true;

        static void Main(string[] args)
        {
            /*
            using (ProcessHostManager processHostManager = new ProcessHostManager("localhost", "orationi", "orationi"))
            {

                if (ShouldProcessSingleRequestCode)
                {
                    SingleRequestCodeProcessing(processHostManager, SingleProcessingRequestCode);
                }
                else
                {
                    MultipleRequestCodesProcessing(processHostManager);
                }

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
            */
            using (var callbackHostManager = new CallbackHostManager(30))
            {
                callbackHostManager.Start();

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }

        /// <summary>
        /// Process single plugin. Method for debugging mostly.
        /// </summary>
        /// <param name="processHostManager">RabbitMQ Instance</param>
        /// <param name="requestCode">Reqeust code number to be processed</param>
        private static void SingleRequestCodeProcessing(ProcessHostManager processHostManager, int requestCode)
        {
            processHostManager.Add(requestCode, IsSync);

            Console.WriteLine($"Processing RequestCode {requestCode}");
            Console.WriteLine("Press [enter] to stop.");
            Console.ReadLine();

            processHostManager.Remove(requestCode, IsSync);
        }

        /// <summary>
        /// Process all plugins in DB.
        /// </summary>
        /// <param name="processHostManager">RabbitMQ Instance</param>
        private static void MultipleRequestCodesProcessing(ProcessHostManager processHostManager)
        {
            List<(int RequestCode, bool IsSync)> plugins = plugins = new List<(int RequestCode, bool IsSync)>();

            using (var dbContext = new OrationiDatabaseContext())
            {
                foreach (var requestCode in dbContext.RequestCodes)
                {
                    plugins.AddRange(new[] { (requestCode.Id, true), (requestCode.Id, false) });
                }
            }

            plugins.ForEach(p => processHostManager.Add(p.RequestCode, p.IsSync));

            Console.WriteLine(" Press [enter] to stop.");
            Console.ReadLine();

            //TODO: разобраться почему после нажатия Enter не происходит завершения работы хост менеджера
            plugins.ForEach(p => processHostManager.Remove(p.RequestCode, p.IsSync));
        }
    }
}