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
        public static ConcurrentDictionary<string, IPluginHost> hostsDictionary = new ConcurrentDictionary<string, IPluginHost>();

        static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));
            AssembliesPreLoader.Execute();

            SynchronousPluginHost sph = new SynchronousPluginHost("localhost", "orationi", "orationi", 1);
            IPluginHost host = hostsDictionary.GetOrAdd(sph.QueryCode, sph);
            host.Run();
            AsynchronousPluginHost aph = new AsynchronousPluginHost("localhost", "orationi", "orationi", 1);
            host = hostsDictionary.GetOrAdd(aph.QueryCode, aph);
            host.Run();

            Task.Run(() =>
            {
                while (true)
                {
                    foreach (var hostKey in hostsDictionary.Keys)
                    {
                        IPluginHost checkedHost = hostsDictionary[hostKey];
                        if (checkedHost.CloseReason != null)
                        {
                            Console.WriteLine($"{checkedHost.IsSynchronous} Remove host. (Reason:{checkedHost.CloseReason})");
                            if (hostsDictionary.TryRemove(hostKey, out checkedHost))
                            {
                                Thread.Sleep(10000);
                                Console.WriteLine("Attach new host");
                                BasePluginHost bph;
                                if (checkedHost.IsSynchronous)
                                {
                                    bph = new SynchronousPluginHost("localhost", "orationi", "orationi", 1);
                                    host = hostsDictionary.GetOrAdd(bph.QueryCode, bph);
                                }
                                else
                                {
                                    Thread.Sleep(10000);
                                    bph = new AsynchronousPluginHost("localhost", "orationi", "orationi", 1);
                                    host = hostsDictionary.GetOrAdd(bph.QueryCode, bph);
                                }

                                Thread.Sleep(1000);
                                host.Run();
                            }
                        }
                    }
                    Thread.Sleep(5000);
                }
            });

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
