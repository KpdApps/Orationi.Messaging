using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Text;
using KpdApps.Orationi.Messaging.Common.Models;

namespace KpdApps.Orationi.Messaging.Core
{
    public class RabbitClient : IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _replyQueueName;
        private readonly EventingBasicConsumer _consumer;
        private readonly BlockingCollection<string> _respQueue = new BlockingCollection<string>();
        private readonly IBasicProperties _props;

        private bool _isSynchronous;

        public RabbitClient(int requestCode, bool isSynchronous)
        {
            _isSynchronous = isSynchronous;

            var factory = new ConnectionFactory() { HostName = "localhost", UserName = "orationi", Password = "orationi" };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _replyQueueName = _channel.QueueDeclare().QueueName;
            _consumer = new EventingBasicConsumer(_channel);

            _props = _channel.CreateBasicProperties();
            var correlationId = Guid.NewGuid().ToString();
            _props.CorrelationId = correlationId;
            _props.ReplyTo = _replyQueueName;

            _consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var response = Encoding.UTF8.GetString(body);
                if (ea.BasicProperties.CorrelationId == correlationId)
                {
                    _respQueue.Add(response);
                }
            };
        }

        public string Execute(int requestCode, Guid messageId)
        {
            RabbitRequest request = new RabbitRequest();
            request.MessageId = messageId;
            request.RequestCode = requestCode;

            string message = JsonConvert.SerializeObject(request);
            string queueName = $"queue-{requestCode}-1";

            _channel.QueueDeclare(queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var messageBytes = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(
                exchange: "",
                routingKey: queueName,
                basicProperties: _props,
                body: messageBytes);

            _channel.BasicConsume(
                consumer: _consumer,
                queue: _replyQueueName,
                autoAck: true);

            return _respQueue.Take(); ;
        }

        public void PullMessage(int requestCode, Guid messageId)
        {
            var factory = new ConnectionFactory() { HostName = "localhost", UserName = "orationi", Password = "orationi" };
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
            _connection.Close();
        }

        public void Dispose()
        {
            if (_connection.IsOpen)
                _connection.Close();

            _connection.Dispose();
        }
    }
}
