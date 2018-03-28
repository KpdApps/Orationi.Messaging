using KpdApps.Orationi.Messaging.Core.Models;
using KpdApps.Orationi.Messaging.ServerCore.ProcessHosts;
using KpdApps.Orationi.Messaging.ServerCore.Workflow;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace KpdApps.Orationi.Messaging.ServerCore.ProcessHosts
{
    public class AsynchronousProcessHost : ProcessHostBase, IProcessHost
    {
        public override bool IsSynchronous => false;

        public override string QueueCode => $"queue-{RequestCode}-{Convert.ToInt32(IsSynchronous)}";

        public AsynchronousProcessHost(string hostname, string username, string password, int requestcode)
            : base(hostname, username, password, requestcode)
        {

        }

        public override void Run()
        {
            ConnectionFactory factory = new ConnectionFactory()
            {
                HostName = _hostname,
                UserName = _username,
                Password = _password
            };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: QueueCode, 
                durable: true, 
                exclusive: false, 
                autoDelete: false, 
                arguments: null);
            channel.BasicQos(0, 1, false);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += Consumer_Received;
            channel.BasicConsume(
                queue: QueueCode, 
                autoAck: false,
                consumer: consumer);
            Console.WriteLine($"{QueueCode} [x] Awaiting async requests");
            
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs ea)
        {
            Console.WriteLine($"Received message from {ea.RoutingKey}");
            Task.Run(() =>
            {
                var body = ea.Body;
                var props = ea.BasicProperties;
                var replyProps = channel.CreateBasicProperties();
                replyProps.CorrelationId = props.CorrelationId;

                try
                {
                    var message = Encoding.UTF8.GetString(body);
                    RabbitRequest rabbitRequest = JsonConvert.DeserializeObject<RabbitRequest>(message);

                    WorkflowProcessor processor = new WorkflowProcessor(rabbitRequest.MessageId, rabbitRequest.RequestCode);
                    processor.Run();

                    Console.WriteLine($" [{QueueCode}] ({message})");
                    rabbitRequest.RequestCode++;
                }
                catch (Exception e)
                {
                    Console.WriteLine($" [{QueueCode}] " + e.Message);
                }
                finally
                {
                    if (CloseReason == null)
                    {
                        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    }
                }
            });
        }
    }
}
