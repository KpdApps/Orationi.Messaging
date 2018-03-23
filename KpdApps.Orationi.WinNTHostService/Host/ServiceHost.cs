using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;

namespace KpdApps.Orationi.WinNTHostService.Host
{
    public class ServiceHost : WebHostService
    {
		public ServiceHost(IWebHost webHost) : base(webHost)
        {
        }

        /// <summary>
        /// Этап, когда служба запускается
        /// </summary>
        /// <param name="args">Аргументы передаваемые при запуске службы</param>
        protected override void OnStarting(string[] args)
        {
            base.OnStarting(args);
            //Helpers.Helpers.WriteToFile($"{DateTime.Now} - OnStarting");
			Program.log.Info("OnStarting");
		}

        /// <summary>
        /// Эпат, когда служба запущена
        /// </summary>
        protected override void OnStarted()
        {
            base.OnStarted();
            //Helpers.Helpers.WriteToFile($"{DateTime.Now} - OnStarted");
			Program.log.Info("OnStarted");
		}

        /// <summary>
        /// Этап, когда служба останавливается
        /// </summary>
        protected override void OnStopping()
        {
            base.OnStopping();
            //Helpers.Helpers.WriteToFile($"{DateTime.Now} - OnStopping");
			Program.log.Info("OnStopping");
		}

        /// <summary>
        /// Этап, когда служба остановлена
        /// </summary>
        protected override void OnStopped()
        {
            base.OnStopped();
            //Helpers.Helpers.WriteToFile($"{DateTime.Now} - OnStopped");
			Program.log.Info("OnStopped");
		}
    }
}