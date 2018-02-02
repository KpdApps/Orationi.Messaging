using System.Linq;

namespace KpdApps.Orationi.Messaging
{
    public class MessagingService : IMessagingService
    {
        public string Ping(string msg)
        {
            return $"{msg}:{msg.Reverse()}";
        }
    }
}
