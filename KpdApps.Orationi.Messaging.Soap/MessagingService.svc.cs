using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web;
using KpdApps.Orationi.Messaging.Common.Models;
using KpdApps.Orationi.Messaging.Core;
using KpdApps.Orationi.Messaging.DataAccess;

namespace KpdApps.Orationi.Messaging.Soap
{
    public class MessagingService : IMessagingService
    {
        private readonly OrationiDatabaseContext _dbContext;
        private readonly HttpContext _httpContext;

        public MessagingService()
        {
            _dbContext = new OrationiDatabaseContext();
            _httpContext = null;
        }

        public Response GetStatus(Guid requestId)
        {
            throw new NotImplementedException();
        }

        public Response ExecuteRequest(Request request)
        {
            IncomingMessageProcessor imp = new IncomingMessageProcessor(_dbContext, _httpContext);
            Response response = imp.Execute(request);
            return response;
        }

        public Response GetResponse(Guid requestId)
        {
            IncomingMessageProcessor imp = new IncomingMessageProcessor(_dbContext, _httpContext);
            Response response = imp.GetResponse(requestId);
            return response;
        }

        public ResponseId SendRequest(Request request)
        {
            throw new NotImplementedException();
        }

        public ResponseId ExecuteRequestAsync(Request request)
        {
            WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Forbidden;
            return new ResponseId
            {
                Error = "Unauthorized",
                IsError = true,
                Id = Guid.NewGuid()
            };

            IncomingMessageProcessor imp = new IncomingMessageProcessor(_dbContext, _httpContext);
            ResponseId response = imp.ExecuteAsync(request);
            return response;
        }

        public string GetVersion()
        {
            return "v.1.0";
        }
    }
}
