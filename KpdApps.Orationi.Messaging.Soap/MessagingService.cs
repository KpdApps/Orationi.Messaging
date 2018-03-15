using System;
using System.ServiceModel;
using KpdApps.Orationi.Messaging.Core;
using KpdApps.Orationi.Messaging.DataAccess;
using KpdApps.Orationi.Messaging.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;

namespace KpdApps.Orationi.Messaging
{
    public class MessagingService : IMessagingService
    {
        private readonly OrationiMessagingContext _dbContext;
        private readonly HttpContext _httpContext;

        public MessagingService(OrationiMessagingContext dbContext, IHttpContextAccessor httpContext)
        {
            _dbContext = dbContext;
            _httpContext = httpContext.HttpContext;
        }

        public Response ExecuteRequest(Request request)
        {
            var imp = new IncomingMessageProcessor(_dbContext, _httpContext);
            var response = imp.Execute(request);
            return response;
        }

        public Response GetResponse(Guid requestId)
        {
            var imp = new IncomingMessageProcessor(_dbContext, _httpContext);
            var response = imp.GetResponse(requestId);
            return response;
        }

        public ResponseId SendRequest(Request request)
        {
            throw new NotImplementedException();
        }

        public ResponseId ExecuteRequestAsync(Request request)
        {
            var imp = new IncomingMessageProcessor(_dbContext, _httpContext);
            var response = imp.ExecuteAsync(request);
            return response;
        }

        public string GetVersion()
        {
            return "v.1.0";
        }
    }
}
