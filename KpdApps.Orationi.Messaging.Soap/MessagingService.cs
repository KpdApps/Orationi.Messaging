using System;
using System.Linq;
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

        public Response ExecuteRequest(Request request)
        {
            // Отдаем запрос в процессор, дальше он сам
            IncomingMessageProcessor imp = new IncomingMessageProcessor(_dbContext);
            Response response = imp.Execute(request);
            return response;
        }

        public Response GetResponse(Guid requestId)
        {
            IncomingMessageProcessor imp = new IncomingMessageProcessor(_dbContext);
            Response response = imp.GetResponse(requestId);
            return response;
        }

        public ResponseId SendRequest(Request request)
        {
            throw new NotImplementedException();
        }

        public ResponseId ExecuteRequestAsync(Request request)
        {
            IncomingMessageProcessor imp = new IncomingMessageProcessor(_dbContext);
            ResponseId response = imp.ExecuteAsync(request);
            return response;
        }

        public string GetVersion()
        {
            return "v.1.0";
        }
    }
}
