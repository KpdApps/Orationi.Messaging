using KpdApps.Orationi.Messaging.Core;
using KpdApps.Orationi.Messaging.DataAccess;
using KpdApps.Orationi.Messaging.DataAccess.Models;
using KpdApps.Orationi.Messaging.DummyPlugins;
using KpdApps.Orationi.Messaging.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KpdApps.Orationi.Messaging.DataAccess.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace KpdApps.Orationi.Messaging.ClientConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            var dbContext = new OrationiMessagingContext(new OrationiContextOptionsBuilder(configuration));

            var imp = new IncomingMessageProcessor(dbContext);

            while (true)
            {
                var dummyRequest = new DummyRequest
                {
                    MessageId = Guid.NewGuid().ToString(),
                    RequestCode = 1
                };

                var request = new Request()
                {
                    RequestBody = dummyRequest.Serialize(),
                    RequestCode = 1,
                    RequestSystemName = "Dummy",
                    RequestUserName = "Dummy"
                };

                Console.WriteLine($" ==> {JsonConvert.SerializeObject(request)}");
                var response = imp.Execute(request);

                Console.WriteLine($" <== {JsonConvert.SerializeObject(response)}");
                Thread.Sleep(1000);
            }
        }
    }
}
