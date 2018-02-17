using System;
using System.Linq;
using KpdApps.Orationi.Messaging.Models;

namespace KpdApps.Orationi.Messaging
{
    public class MessagingService : IMessagingService
    {
        public Response ExecuteRequest(Request request)
        {
            throw new NotImplementedException();
        }

        public ResponseId ExecuteRequestAsync(Request request)
        {
            throw new NotImplementedException();
        }

        public Response GetResponse(Guid requestId)
        {
            throw new NotImplementedException();
        }

        public ResponseId SendRequest(Request request)
        {
            throw new NotImplementedException();
        }
    }
}
