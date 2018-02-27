using KpdApps.Orationi.Messaging.Core.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace KpdApps.Orationi.Messaging.ServerCore.PluginHosts
{
    public class SynchronousPluginHost : BasePluginHost
    {
        public override bool IsSynchronous => true;

        public override string QueueCode => $"queue-{RequestCode}-{Convert.ToInt32(IsSynchronous)}";

        public SynchronousPluginHost(string hostname, string username, string password, int requestcode)
            : base(hostname, username, password, requestcode)
        {

        }

        public override void Run()
        {
            ConnectionFactory factory = new ConnectionFactory() { HostName = _hostname, UserName = _username, Password = _password };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            channel.QueueDeclare(queue: "rpc_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            channel.BasicQos(0, 1, false);
            var consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume(queue: "rpc_queue", autoAck: false, consumer: consumer);
            Console.WriteLine($"{QueueCode} [x] Awaiting RPC requests");
            consumer.Received += Consumer_Received;
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs ea)
        {
            Task.Run(() =>
            {
                string response = null;

                var body = ea.Body;
                var props = ea.BasicProperties;
                var replyProps = channel.CreateBasicProperties();
                replyProps.CorrelationId = props.CorrelationId;

                try
                {
                    var message = Encoding.UTF8.GetString(body);
                    RabbitRequest rabbitRequest = JsonConvert.DeserializeObject<RabbitRequest>(message);

                    Random r = new Random(DateTime.Now.Millisecond);
                    if (r.Next(1, 100) == 8)
                    {
                        Dispose();
                    }

                    Pipeline.Pipeline pipeline = new Pipeline.Pipeline(rabbitRequest.MessageId, rabbitRequest.RequestCode);
                    pipeline.Init();
                    pipeline.Run();

                    Console.WriteLine($" [{QueueCode}] ({message})");
                    rabbitRequest.RequestCode++;
                    response = JsonConvert.SerializeObject(rabbitRequest);
                }
                catch (Exception e)
                {
                    Console.WriteLine($" [{QueueCode}] " + e.Message);
                    response = "";
                }
                finally
                {
                    var responseBytes = Encoding.UTF8.GetBytes(response);
                    if (CloseReason == null)
                    {
                        channel.BasicPublish(exchange: "", routingKey: props.ReplyTo, basicProperties: replyProps, body: responseBytes);
                        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    }
                }
            });
        }
    }
}
