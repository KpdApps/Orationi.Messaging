using RabbitMQ.Client;
using System;

namespace KpdApps.Orationi.Messaging.ServerCore.PluginHosts
{
    public abstract class BasePluginHost : IPluginHost, IDisposable
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

        public BasePluginHost(string hostname, string username, string password, int requestcode)
        {
            _hostname = hostname;
            _username = username;
            _password = password;
            _requestcode = requestcode;
        }

        ~BasePluginHost()
        {
            this.Dispose();
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
