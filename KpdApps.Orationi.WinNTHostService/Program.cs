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

        public static IWebHost BuildWebHost(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("Orationi.WinNTHostService.Configuration.json")
                .Build();

            // По умолчанию служба слушает 5000 порт
            return WebHost.CreateDefaultBuilder(args)
                .UseHttpSys(options =>
                {
                    options.Authentication.AllowAnonymous = true;
                    options.UrlPrefixes.Add(configuration["SeviceHost"]);
                })
                .UseStartup<Startup>()
                .Build();
        }
    }
}
