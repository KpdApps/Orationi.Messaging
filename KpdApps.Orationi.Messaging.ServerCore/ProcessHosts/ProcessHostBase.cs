using RabbitMQ.Client;
using System;

namespace KpdApps.Orationi.Messaging.ServerCore.ProcessHosts
{
    public abstract class ProcessHostBase : IProcessHost, IDisposable
    {
        protected string _hostname;
        protected string _username;
        protected string _password;
        protected int _requestcode;
        public int RequestCode
        {
            get { return _requestcode; }
        }

        public string CloseReason
        {
            get
            {
                if (channel.CloseReason != null)
                {
                    return channel.CloseReason.ReplyText;
                }

                if (connection.CloseReason != null)
                {
                    return connection.CloseReason.ReplyText;
                }

                return null;
            }
        }

        public abstract bool IsSynchronous { get; }

        public abstract string QueueCode { get; }

        protected IConnection connection;
        protected IModel channel;

        public ProcessHostBase(string hostname, string username, string password, int requestcode)
        {
            _hostname = hostname;
            _username = username;
            _password = password;
            _requestcode = requestcode;
        }

        public void Dispose()
        {
            if (channel.CloseReason == null)
            {
                channel.Close();
            }

            if (connection.CloseReason == null)
            {
                connection.Close();
            }

            channel.Dispose();
            connection.Dispose();
        }

        public abstract void Run();
    }
}
