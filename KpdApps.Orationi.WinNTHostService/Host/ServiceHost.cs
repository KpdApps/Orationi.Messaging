using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.Extensions.Logging;

namespace KpdApps.Orationi.WinNTHostService.Host
{
    public class ServiceHost : WebHostService
    {
        private readonly ILogger log;

		public ServiceHost(IWebHost webHost) : base(webHost)
		{
            var loggerFactory = (ILoggerFactory)webHost.Services.GetService(typeof(ILoggerFactory));
            log = loggerFactory.CreateLogger(this.GetType());
        }

        /// <summary>
        /// Этап, когда служба запускается
        /// </summary>
        /// <param name="args">Аргументы передаваемые при запуске службы</param>
        protected override void OnStarting(string[] args)
        {
            base.OnStarting(args);
			log.LogInformation("OnStarting");
		}

        /// <summary>
        /// Эпат, когда служба запущена
        /// </summary>
        protected override void OnStarted()
        {
            base.OnStarted();
			log.LogInformation("OnStarted");
		}

        /// <summary>
        /// Этап, когда служба останавливается
        /// </summary>
        protected override void OnStopping()
        {
            base.OnStopping();
			log.LogInformation("OnStopping");
		}

        /// <summary>
        /// Этап, когда служба остановлена
        /// </summary>
        protected override void OnStopped()
        {
            base.OnStopped();
			log.LogInformation("OnStopped");
		}
    }
}