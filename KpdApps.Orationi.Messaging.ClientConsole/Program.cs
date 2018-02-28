using KpdApps.Orationi.Messaging.Core;
using KpdApps.Orationi.Messaging.DataAccess;
using KpdApps.Orationi.Messaging.DataAccess.Models;
using KpdApps.Orationi.Messaging.DummyPlugins;
using KpdApps.Orationi.Messaging.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KpdApps.Orationi.Messaging.ClientConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            DbContextOptionsBuilder<OrationiMessagingContext> optionsBuilder = new DbContextOptionsBuilder<OrationiMessagingContext>();
            optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=OrationiMessageBus;Integrated Security=True;");
            OrationiMessagingContext dbContext = new OrationiMessagingContext(optionsBuilder.Options);

            IncomingMessageProcessor imp = new IncomingMessageProcessor(dbContext);

            while (true)
            {
                DummyRequest dummyRequest = new DummyRequest();
                dummyRequest.MessageId = Guid.NewGuid().ToString();
                dummyRequest.RequestCode = 1;

                Request request = new Request()
                {
                    RequestBody = dummyRequest.Serialize(),
                    RequestCode = 1,
                    RequestSystemName = "Dummy",
                    RequestUserName = "Dummy"
                };

                Console.WriteLine($" ==> {JsonConvert.SerializeObject(request)}");
                Response response = imp.Execute(request);

                Console.WriteLine($" <== {JsonConvert.SerializeObject(response)}");
                Thread.Sleep(1000);
            }
        }
    }
}
