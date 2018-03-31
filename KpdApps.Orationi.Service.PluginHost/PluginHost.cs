using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;

namespace KpdApps.Orationi.Service.PluginHost
{
    public partial class PluginHost : ServiceBase
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(PluginHost));

        public PluginHost()
        {
            InitializeComponent();
            XmlConfigurator.Configure();
        }

        protected override void OnStart(string[] args)
        {
            log.Info("Service started");
        }

        protected override void OnStop()
        {
            log.Info("Service stopped");
        }
    }
}
