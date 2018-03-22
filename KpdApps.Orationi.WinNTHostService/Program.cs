using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using KpdApps.Orationi.WinNTHostService.Host;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace KpdApps.Orationi.WinNTHostService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).RunAsServiceHost();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            // По умолчанию служба слушает 5000 порт
            WebHost.CreateDefaultBuilder(args)
                //.UseHttpSys(options =>
                //{
                //    options.Authentication.AllowAnonymous = true;
                //    options.UrlPrefixes.Add("http://localhost:7000");
                //})
                .UseStartup<Startup>()
                .Build();
    }
}
