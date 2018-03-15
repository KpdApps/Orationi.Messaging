using System;
using KpdApps.Orationi.Messaging.Core;
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

        public Response GetStatus(Guid requestId)
        {
            throw new NotImplementedException();
        }

        public Response ExecuteRequest(Request request)
        {
            var imp = new IncomingMessageProcessor(_dbContext);
            var response = imp.Execute(request);
            return response;
        }

        public Response GetResponse(Guid requestId)
        {
            var imp = new IncomingMessageProcessor(_dbContext);
            var response = imp.GetResponse(requestId);
            return response;
        }

        public ResponseId SendRequest(Request request)
        {
            throw new NotImplementedException();
        }

        public ResponseId ExecuteRequestAsync(Request request)
        {
            var imp = new IncomingMessageProcessor(_dbContext);
            var response = imp.ExecuteAsync(request);
            return response;
        }

        public string GetVersion()
        {
            return "v.1.0";
        }
    }
}
