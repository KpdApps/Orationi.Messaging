using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Text;
using KpdApps.Orationi.Messaging.Common.Models;
using KpdApps.Orationi.Messaging.Core.Configurations.Rabbitmq;

namespace KpdApps.Orationi.Messaging.Core
{
    public class RabbitClient : IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _correlationId;
        private readonly BlockingCollection<string> _respQueue = new BlockingCollection<string>();
        private readonly string _hostName;
        private readonly string _userName;
        private readonly string _password;


        public RabbitClient()
        {

            var rabbitmqCongig = RabbitmqConfigurationSection.GetConfiguration();
            _hostName = rabbitmqCongig.HostName;
            _userName = rabbitmqCongig.UserName;
            _password = rabbitmqCongig.Password;

            var factory = new ConnectionFactory() { HostName = _hostName, UserName = _userName, Password = _password };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _correlationId = Guid.NewGuid().ToString();
        }

        public string Execute(int requestCode, Guid messageId)
        {
            RabbitRequest request = new RabbitRequest
            {
                MessageId = messageId,
                RequestCode = requestCode
            };

            string message = JsonConvert.SerializeObject(request);
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);

            string queueName = $"queue-{requestCode}-1";

            _channel.QueueDeclare(queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            IBasicProperties props = _channel.CreateBasicProperties();
            props.CorrelationId = _correlationId;
            string replyQueueName = _channel.QueueDeclare($"response-{requestCode}-1-{messageId.ToString()}").QueueName;
            props.ReplyTo = replyQueueName;

            

            _channel.BasicPublish(
                exchange: "",
                routingKey: queueName,
                basicProperties: props,
                body: messageBytes);

            EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var response = Encoding.UTF8.GetString(body);
                if (ea.BasicProperties.CorrelationId == _correlationId)
                {
                    _respQueue.Add(response);
                }
            };

            _channel.BasicConsume(
                consumer: consumer,
                queue: replyQueueName,
                autoAck: true);

            return _respQueue.Take();
        }

        public void PullMessage(int requestCode, Guid messageId)
        {
            var factory = new ConnectionFactory() { HostName = _hostName, UserName = _userName, Password = _password };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    string queueName = $"queue-{requestCode}-0";

                    channel.QueueDeclare(queue: queueName,
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;

                    RabbitRequest request = new RabbitRequest()
                    {
                        MessageId = messageId,
                        RequestCode = requestCode
                    };

                    string message = JsonConvert.SerializeObject(request);

                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                                         routingKey: queueName,
                                         basicProperties: properties,
                                         body: body);
                    Console.WriteLine(" [x] Sent {0}", message);
                }
            }
        }

        public void Close()
        {
            if (_connection.IsOpen)
            {
                _connection.Close();
            }
        }

        public void Dispose()
        {
            Close();
            _connection.Dispose();
        }
    }
}
