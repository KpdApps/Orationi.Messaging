using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KpdApps.Orationi.Messaging.ServerCore.PluginHosts;

namespace KpdApps.Orationi.Messaging.ServerCore.ProcessHosts
{
    public class ProcessHostManager : IDisposable
    {
        private object _locker = new object();

        public static ConcurrentDictionary<string, IProcessHost> _hostsDictionary = new ConcurrentDictionary<string, IProcessHost>();
        private List<ManagedPlugin> _managedPlugins = new List<ManagedPlugin>();

        private string _hostname;
        private string _username;
        private string _password;

        private readonly AutoResetEvent _ping = new AutoResetEvent(false);
        private readonly TimeSpan _pingInterval = TimeSpan.FromSeconds(10);
        private readonly CancellationTokenSource _shutdown = new CancellationTokenSource();

        public ProcessHostManager(string hostname, string username, string password)
        {
            _hostname = hostname;
            _username = username;
            _password = password;

            AssembliesPreLoader.Execute();

            var token = _shutdown.Token;
            Task.Run(
                () =>
                {
                    while (!token.IsCancellationRequested)
                    {
                        lock (_locker)
                        {
                            foreach (ManagedPlugin managedPlugin in _managedPlugins)
                            {
                                if (!_hostsDictionary.TryGetValue(managedPlugin.QueueCode, out IProcessHost host))
                                {
                                    host = ProcessHostFactory.GetPluginHost(_hostname, _username, _password, managedPlugin.RequestCode, managedPlugin.IsSynchronous);
                                    host.Run();
                                    _hostsDictionary.GetOrAdd(managedPlugin.QueueCode, host);
                                    continue;
                                }

                                if (string.IsNullOrEmpty(host.CloseReason))
                                    continue;

                                _hostsDictionary.TryRemove(host.QueueCode, out host);
                            }
                        }

                        lock (_locker)
                        {
                            foreach (string key in _hostsDictionary.Keys)
                            {
                                if (!_managedPlugins.Any(m => m.QueueCode == key))
                                {
                                    IProcessHost host;
                                    _hostsDictionary.TryRemove(key, out host);
                                }
                            }
                        }

                        _ping.WaitOne(_pingInterval);
                    }
                },
                token
            );
        }

        public void Add(int requestCode, bool isSynchronous)
        {
            lock (_locker)
            {
                ManagedPlugin managedPlugin = _managedPlugins.FirstOrDefault(m => m.RequestCode == requestCode && m.IsSynchronous == isSynchronous);
                if (managedPlugin != null)
                {
                    return;
                }

                _managedPlugins.Add(new ManagedPlugin
                {
                    RequestCode = requestCode,
                    IsSynchronous = isSynchronous
                });

            }
        }

        public void Remove(int requestCode, bool isSynchronous)
        {
            lock (_locker)
            {
                ManagedPlugin managedPlugin = _managedPlugins.FirstOrDefault(m => m.RequestCode == requestCode && m.IsSynchronous == isSynchronous);
                if (managedPlugin == null)
                {
                    return;
                }

                _managedPlugins.Remove(managedPlugin);
            }
        }

        public void Dispose()
        {
            _shutdown.Cancel();
        }
    }
}
