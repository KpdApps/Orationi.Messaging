using System;
using System.Collections.Generic;
using System.Text;
using KpdApps.Orationi.Messaging.DataAccess.Services;
using Microsoft.Extensions.Configuration;

namespace KpdApps.Orationi.Messaging.ServerCore.Helpers
{
    public static class ContextOptionsBuilderExtensions
    {
        public static IContextOptionsBuilder GetContextOptionsBuilder()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            return new OrationiContextOptionsBuilder(configuration);
        }
    }
}