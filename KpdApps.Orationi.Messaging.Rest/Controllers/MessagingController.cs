using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using KpdApps.Orationi.Messaging.Core;
using KpdApps.Orationi.Messaging.DataAccess;

namespace KpdApps.Orationi.Messaging.Rest.Controllers
{
    [RoutePrefix("api/rest/messaging")]
    public class MessagingController : ApiController, IMessagingService
    {
        private OrationiDatabaseContext _dbContext;

        public MessagingController()
        {
            _dbContext = new OrationiDatabaseContext();
        }

        [HttpGet]
        [Route("version")]
        public string GetVersion()
        {
            return "v.1.0";
        }

        [HttpGet]
        [Route("{requestId}")]
        public Common.Models.Response GetResponse(Guid requestId)
        {
            if (!AuthorizeHelpers.IsAuthorized(_dbContext,
                GetTokenValue(),
                requestId,
                out Common.Models.Response response,
                out var externalSystem))
            {
                ActionContext.Response.StatusCode = HttpStatusCode.Forbidden;
                return response;
            }

            IncomingMessageProcessor imp = new IncomingMessageProcessor(_dbContext, externalSystem);
            response = imp.GetResponse(requestId);
            return response;
            
        }

        [HttpGet]
        [Route("status/{requestId}")]
        public Common.Models.Response GetStatus(Guid requestId)
        {
            throw new NotImplementedException();
        }
        
        [HttpPost]
        [Route("sync")]
        public Common.Models.Response ExecuteRequest([FromBody]Common.Models.Request request)
        {
            if (!AuthorizeHelpers.IsAuthorized(
                _dbContext,
                GetTokenValue(),
                request.Code,
                out Common.Models.Response response,
                out var externalSystem))
            {
                ActionContext.Response.StatusCode = HttpStatusCode.Forbidden;
                return response;
            }

            IncomingMessageProcessor imp = new IncomingMessageProcessor(_dbContext, externalSystem);
            response = imp.Execute(request);
            return response;

        }

        [HttpPost]
        [Route("async")]
        public Common.Models.ResponseId ExecuteRequestAsync([FromBody]Common.Models.Request request)
        {
            if (!AuthorizeHelpers.IsAuthorized(_dbContext,
                GetTokenValue(),
                request.Code,
                out Common.Models.ResponseId response,
                out var externalSystem))
            {
                ActionContext.Response.StatusCode = HttpStatusCode.Forbidden;
                return response;
            }

            IncomingMessageProcessor imp = new IncomingMessageProcessor(_dbContext, externalSystem);
            response = imp.ExecuteAsync(request);
            return response;
        }

        [HttpPost]
        [Route("request")]
        public Common.Models.ResponseId SendRequest([FromBody]Common.Models.Request request)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("xsd/{requestCode}")]
        public Common.Models.Response GetXsd(int requestCode)
        {
            throw new NotImplementedException();
        }

        [NonAction]
        private string GetTokenValue()
        {
            return Request.Headers.GetValues("Token").FirstOrDefault();
        }
    }
}
