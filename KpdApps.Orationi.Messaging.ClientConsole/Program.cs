using KpdApps.Orationi.Messaging.Core;
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
                Random r = new Random();
                Thread.Sleep(r.Next(10, 1000));
                Guid guid = Guid.NewGuid();
                Console.WriteLine($"{count}){guid}");
                Console.WriteLine($"{count}){client.Execute(1, guid)}");
            }
        }
    }
}
