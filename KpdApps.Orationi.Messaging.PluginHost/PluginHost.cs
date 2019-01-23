using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceProcess;
using KpdApps.Orationi.Messaging.Core.Configurations.Rabbitmq;
using KpdApps.Orationi.Messaging.DataAccess;
using KpdApps.Orationi.Messaging.ServerCore.ProcessHosts;
using log4net;
using log4net.Config;

namespace KpdApps.Orationi.Messaging.PluginHost
{
    public partial class PluginHost : ServiceBase
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(PluginHost));
        private readonly ProcessHostManager processHostManager;
        private readonly List<Plugin> plugins;

        public PluginHost()
        {
            XmlConfigurator.Configure();
            var stopWatch = Stopwatch.StartNew();
            log.Debug("Инициализация...");
            InitializeComponent();
            var rabbitmqConfig = RabbitmqConfigurationSection.GetConfiguration();
            processHostManager = new ProcessHostManager(rabbitmqConfig.HostName, rabbitmqConfig.UserName, rabbitmqConfig.Password);
            plugins = new List<Plugin>();

            using (var dbContext = new OrationiDatabaseContext())
            {
                foreach (var requestCode in dbContext.RequestCodes)
                {
                    log.Debug($"Загрузка синхронного/асинхронного обработчиков для кода запроса \"{requestCode.Id}\"");
                    plugins.AddRange(new[] {
                        new Plugin
                        {
                            RequestCode = requestCode.Id,
                            IsSync = true
                        },
                        new Plugin
                        {
                            RequestCode = requestCode.Id,
                            IsSync = false
                        }
                    });
                }
            }
            stopWatch.Stop();
            log.Debug($"Инициализация выполнилась за {stopWatch.Elapsed.TotalSeconds} секунд");
        }

        protected override void OnStart(string[] args)
        {
            plugins.ForEach(p =>
            {
                log.Debug($"Запуск {(p.IsSync ? "синхронного" : "асинхронного")} обработчика для кода запроса \"{p.RequestCode}\"");
                processHostManager.Add(p.RequestCode, p.IsSync);
            });
            log.Info("Служба запущена");
        }

        protected override void OnStop()
        {
            plugins.ForEach(p =>
            {
                log.Debug($"Остановка {(p.IsSync ? "синхронного" : "асинхронного")} обработчика для кода запроса \"{p.RequestCode}\"");
                processHostManager.Remove(p.RequestCode, p.IsSync);
            });
            log.Info("Служба остановлена");
        }
    }
}
