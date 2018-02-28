using KpdApps.Orationi.Messaging.Core;
using KpdApps.Orationi.Messaging.DataAccess;
using KpdApps.Orationi.Messaging.DataAccess.Models;
using KpdApps.Orationi.Messaging.DummyPlugins;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KpdApps.Orationi.Messaging.ClientConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            RabbitClient client = new RabbitClient(1, true);
            int count = 0;

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
            Console.ReadKey();
            /*
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

                if (count % 2 == 0)
                {
                    Task.Run(() =>
                    {
                        Console.WriteLine($"{count}){message.Id}");
                        Console.WriteLine($"{count}){client.Execute(1, message.Id)}");
                    });
                }
                else
                {
                    Console.WriteLine($"{count}) A {message.Id}");
                    client.PullMessage(1, message.Id);
                }

                if (count % 100 == 0)
                {
                    Console.Clear();
                }
            }*/
        }
    }
}
