using System;
using System.Linq;
using KpdApps.Orationi.Messaging.DataAccess;
using KpdApps.Orationi.Messaging.Models;

namespace KpdApps.Orationi.Messaging
{
    public class MessagingService : IMessagingService
    {
        OrationiMessagingContext _dbContext;

        public MessagingService(OrationiMessagingContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Response ExecuteRequest(Request request)
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

        public ResponseId ExecuteRequestAsync(Request request)
        {
            throw new NotImplementedException();
        }

        public string GetVersion()
        {
            return "v.1.0";
        }
    }
}
