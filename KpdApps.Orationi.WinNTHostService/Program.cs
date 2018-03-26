﻿using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using KpdApps.Orationi.WinNTHostService.Host;
using log4net;
using log4net.Config;


namespace KpdApps.Orationi.WinNTHostService
{
    public class Program
    {
        protected internal static string BasePath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

        public static void Main(string[] args)
        {
            BuildWebHost(args).RunAsServiceHost();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(BasePath)
                .AddJsonFile("Orationi.WinNTHostService.Configuration.json")
                .Build();

            // По умолчанию служба слушает 5000 порт
            return WebHost.CreateDefaultBuilder(args)
                .UseHttpSys(options =>
                {
                    options.Authentication.AllowAnonymous = true;
                    options.UrlPrefixes.Add(configuration["ServiceHost"]);
                })
                .UseStartup<Startup>()
                .Build();
        }
    }
}