using System;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Threading;
using KpdApps.Orationi.Messaging.DataAccess.Models;
using KpdApps.Orationi.Messaging.Sdk.ContractInspector;

namespace KpdApps.Orationi.Messaging.Core
{
    public class ContractInspector : IDisposable
    {
        private readonly Mutex _mutex = new Mutex(false, "KpdApps.Orationi.Messaging.AssemblyInspector.SyncObject");
        private bool _isDispose;
        private static readonly string BasePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Plugins");
        private AppDomain _appDomain;

        private readonly RegisteredPlugin _registeredPlugin;
        private readonly PluginAssembly _pluginAssembly;
        private readonly string _assembliesFolderPath;
        private readonly string _loadingAssemblyPath;

        private ContractInspectorProxyObj _proxyObj;

        public ContractInspector(RegisteredPlugin registeredPlugin)
        {
            _registeredPlugin = registeredPlugin;
            _pluginAssembly = registeredPlugin.PluginAssembly;
            long unixTimeSec = ((DateTimeOffset)_pluginAssembly.Modified).ToUnixTimeSeconds();
            _assembliesFolderPath = Path.Combine(BasePath, $"{_pluginAssembly.Name}-{unixTimeSec}");
            _loadingAssemblyPath = Path.Combine(_assembliesFolderPath, $"{_pluginAssembly.Name}.dll");
        }

        public ContractInspectorProxyObj Inspect()
        {
            if (_proxyObj != null)
                return _proxyObj;

            var zipAssemblyPackage = $"{_loadingAssemblyPath}.zip";

            _mutex.WaitOne();

            if (!Directory.Exists(_assembliesFolderPath))
            {
                Directory.CreateDirectory(_assembliesFolderPath);

                using (var writer = new BinaryWriter(File.OpenWrite(zipAssemblyPackage)))
                {
                    writer.Write(_pluginAssembly.Assembly, 0, _pluginAssembly.Assembly.Length);
                }

                ZipFile.ExtractToDirectory(zipAssemblyPackage, _assembliesFolderPath);

                File.Delete(zipAssemblyPackage);
            }

            var appDomainSetup = new AppDomainSetup
            {
                ApplicationBase = _assembliesFolderPath
            };

            _appDomain = AppDomain.CreateDomain($"PluginsDomain-{Guid.NewGuid()}", AppDomain.CurrentDomain.Evidence, appDomainSetup);

            var assemblyName = AssemblyName.GetAssemblyName(_loadingAssemblyPath);
            var type = typeof(ContractInspectorProxyObj);
            _proxyObj = (ContractInspectorProxyObj)_appDomain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName);
            _proxyObj.Inspect(assemblyName, _registeredPlugin.Class);

            if (_proxyObj.IsError)
            {
                throw _proxyObj.Exception;
            }

            _mutex.ReleaseMutex();

            return _proxyObj;
        }

        public void Dispose()
        {
            if (!_isDispose)
            {
                _isDispose = true;

                if (_appDomain != null)
                    AppDomain.Unload(_appDomain);
            }
        }
    }
}
