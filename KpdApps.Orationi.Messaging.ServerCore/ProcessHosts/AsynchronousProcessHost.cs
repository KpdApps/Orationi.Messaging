using KpdApps.Orationi.Messaging.ServerCore.Workflow;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;
using KpdApps.Orationi.Messaging.Common.Models;
using log4net;

namespace KpdApps.Orationi.Messaging.ServerCore.ProcessHosts
{
    public class AsynchronousProcessHost : ProcessHostBase, IProcessHost
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(AsynchronousProcessHost));

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
            log.Debug($"{QueueCode} [x] Ожидание асинхронного запроса...");
            
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs ea)
        {
            log.Debug($"Получено сообщение от {ea.RoutingKey}");
            Task.Run(() =>
            {
                var body = ea.Body;
                var props = ea.BasicProperties;
                var replyProps = channel.CreateBasicProperties();
                replyProps.CorrelationId = props.CorrelationId;

                try
                {
                    var message = Encoding.UTF8.GetString(body);
                    log.Debug($"Тело:\r\n{message}");
                    RabbitRequest rabbitRequest = JsonConvert.DeserializeObject<RabbitRequest>(message);

                    WorkflowProcessor processor = new WorkflowProcessor(rabbitRequest.MessageId, rabbitRequest.RequestCode);
                    processor.Run();

                    log.Debug($" [{QueueCode}] ({message})");
                    rabbitRequest.RequestCode++;
                }
                catch (Exception ex)
                {
                    log.Fatal($"[{QueueCode}] ({ex.Message})", ex);
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
