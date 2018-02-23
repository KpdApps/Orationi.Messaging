using KpdApps.Orationi.Messaging.Core;
using KpdApps.Orationi.Messaging.DataAccess;
using KpdApps.Orationi.Messaging.DataAccess.Models;
using KpdApps.Orationi.Messaging.DummyPlugins;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;

namespace KpdApps.Orationi.Messaging.ClientConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            RabbitClient client = new RabbitClient();
            int count = 0;
            while (true)
            {
                count++;

                DummyRequest request = new DummyRequest();
                request.MessageId = Guid.NewGuid().ToString();
                request.RequestCode = 1;

                DbContextOptionsBuilder<OrationiMessagingContext> optionsBuilder = new DbContextOptionsBuilder<OrationiMessagingContext>();
                optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=OrationiMessageBus;Integrated Security=True;");//);

                Message message = new Message();
                using (OrationiMessagingContext dbContext = new OrationiMessagingContext(optionsBuilder.Options))
                {
                    message.RequestCode = 1;
                    message.RequestSystem = "ClientConsole";
                    message.RequestUser = "orationi";
                    message.RequestBody = request.Serialize();
                    dbContext.Messages.Attach(message);
                    dbContext.SaveChanges();
                }

                Console.WriteLine($"{count}){message.Id}");
                Console.WriteLine($"{count}){client.Execute(1, message.Id)}");

                if (count % 100 == 0)
                {
                    Console.Clear();
                }
            }
        }
    }
}
