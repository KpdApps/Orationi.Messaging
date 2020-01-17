using System.Configuration;
using System.ServiceProcess;
using KpdApps.Orationi.Messaging.ServerCore.Callback;
using log4net;
using log4net.Config;

namespace KpdApps.Orationi.Messaging.CallbackHost
{
    public partial class CallbackHost : ServiceBase
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(CallbackHost));
        private readonly CallbackHostManager _callbackHostManager;

        public CallbackHost()
        {
            InitializeComponent();
            XmlConfigurator.Configure();
            _callbackHostManager = new CallbackHostManager(
                double.Parse(ConfigurationManager.AppSettings["CheckFrequencyCallbackMessages"]));
        }

        protected override void OnStart(string[] args)
        {
            _callbackHostManager.Start();
            Log.Info("Callback servise was run");
        }

        protected override void OnStop()
        {
            _callbackHostManager.Stop();
            Log.Info("Callback service was stoped");
        }
    }
}
