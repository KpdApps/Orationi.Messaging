using System;
using System.Net;
using System.ServiceModel.Web;
using KpdApps.Orationi.Messaging.Common.Models;
using KpdApps.Orationi.Messaging.Core;
using KpdApps.Orationi.Messaging.DataAccess;

namespace KpdApps.Orationi.Messaging.Soap
{
    public class MessagingService : IMessagingService
    {
        private readonly OrationiDatabaseContext _dbContext;

        public MessagingService()
        {
            _dbContext = new OrationiDatabaseContext();
        }

        public Response GetStatus(Guid requestId)
        {
            throw new NotImplementedException();
        }

        public Response ExecuteRequest(Request request)
        {
            if (!AuthorizeHelpers.IsAuthorized(
                _dbContext,
                WebOperationContext.Current.IncomingRequest.Headers["Token"],
                request.Code,
                out Response response,
                out var externalSystem))
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Forbidden;
                return response;
            }

            IncomingMessageProcessor imp = new IncomingMessageProcessor(_dbContext, externalSystem);
            response = imp.Execute(request);
            return response;
        }

        public Response GetResponse(Guid requestId)
        {
            if (!AuthorizeHelpers.IsAuthorized(_dbContext,
                WebOperationContext.Current.IncomingRequest.Headers["Token"],
                requestId,
                out Response response,
                out var externalSystem))
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Forbidden;
                return response;
            }

            IncomingMessageProcessor imp = new IncomingMessageProcessor(_dbContext, externalSystem);
            response = imp.GetResponse(requestId);
            return response;
        }

        public ResponseId SendRequest(Request request)
        {
            throw new NotImplementedException();
        }

        public ResponseId ExecuteRequestAsync(Request request)
        {
            if (!AuthorizeHelpers.IsAuthorized(_dbContext, 
                WebOperationContext.Current.IncomingRequest.Headers["Token"], 
                request.Code, 
                out ResponseId response, 
                out var externalSystem))
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Forbidden;
                return response;
            }
            
            IncomingMessageProcessor imp = new IncomingMessageProcessor(_dbContext, externalSystem);
            response = imp.ExecuteAsync(request);
            return response;
        }

        public string GetVersion()
        {
            return "v.1.0";
        }
    }
}
