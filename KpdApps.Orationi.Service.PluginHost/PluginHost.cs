using System.Collections.Generic;
using System.ServiceProcess;
using KpdApps.Orationi.Messaging.DataAccess;
using KpdApps.Orationi.Messaging.ServerCore.PluginHosts;
using log4net;
using log4net.Config;

namespace KpdApps.Orationi.Service.PluginHost
{
    public partial class PluginHost : ServiceBase
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(PluginHost));
        private readonly ProcessHostManager processHostManager;
        private readonly List<(int RequestCode, bool IsSync)> plugins;

        public PluginHost()
        {
            InitializeComponent();
            XmlConfigurator.Configure();
            processHostManager = new ProcessHostManager("localhost", "orationi", "orationi");
            plugins = new List<(int RequestCode, bool IsSync)>();

            using (var dbContext = new OrationiDatabaseContext())
            {
                foreach (var requestCode in dbContext.RequestCodes)
                {
                    plugins.AddRange(new [] {(requestCode.Id, true), (requestCode.Id, false)});
                }
            }
        }

        protected override void OnStart(string[] args)
        {
            plugins.ForEach(p => processHostManager.Add(p.RequestCode, p.IsSync));
            log.Info("Service started");
        }

        protected override void OnStop()
        {
            plugins.ForEach(p => processHostManager.Remove(p.RequestCode, p.IsSync));
            log.Info("Service stopped");
        }
    }
}
