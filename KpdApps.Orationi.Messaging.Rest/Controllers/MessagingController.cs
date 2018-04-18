using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Web.Http;
using KpdApps.Orationi.Messaging.Common.Models;
using KpdApps.Orationi.Messaging.Core;
using KpdApps.Orationi.Messaging.DataAccess;
using KpdApps.Orationi.Messaging.Sdk;
using KpdApps.Orationi.Messaging.Sdk.Attributes;
using KpdApps.Orationi.Messaging.Sdk.Plugins;
using TextReader = System.IO.TextReader;

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
        public Response GetResponse(Guid requestId)
        {
            if (!AuthorizeHelpers.IsAuthorized(_dbContext,
                GetTokenValue(),
                requestId,
                out Common.Models.Response response,
                out var externalSystem))
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Forbidden, response));
            }

            IncomingMessageProcessor imp = new IncomingMessageProcessor(_dbContext, externalSystem);
            response = imp.GetResponse(requestId);
            return response;
        }

        [HttpGet]
        [Route("status/{requestId}")]
        public Response GetStatus(Guid requestId)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("sync")]
        public Response ExecuteRequest([FromBody]Common.Models.Request request)
        {
            if (!AuthorizeHelpers.IsAuthorized(
                _dbContext,
                GetTokenValue(),
                request.Code,
                out Common.Models.Response response,
                out var externalSystem))
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Forbidden, response));
            }

            IncomingMessageProcessor imp = new IncomingMessageProcessor(_dbContext, externalSystem);
            response = imp.Execute(request);
            return response;
        }

        [HttpPost]
        [Route("async")]
        public ResponseId ExecuteRequestAsync([FromBody]Common.Models.Request request)
        {
            if (!AuthorizeHelpers.IsAuthorized(_dbContext,
                GetTokenValue(),
                request.Code,
                out Common.Models.ResponseId response,
                out var externalSystem))
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Forbidden, response));
            }

            IncomingMessageProcessor imp = new IncomingMessageProcessor(_dbContext, externalSystem);
            response = imp.ExecuteAsync(request);
            return response;
        }

        [HttpPost]
        [Route("request")]
        public ResponseId SendRequest([FromBody]Common.Models.Request request)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("xsd/{requestCode}")]
        public ResponseXsd GetXsd(int requestCode)
        {
            if (!AuthorizeHelpers.IsAuthorized(
                _dbContext,
                GetTokenValue(),
                requestCode,
                out Common.Models.ResponseXsd response,
                out var externalSystem))
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Forbidden, response));
            }

            var imp = new IncomingMessageProcessor(_dbContext, externalSystem);
            response = imp.GetXsd(requestCode);

            if (response.IsError)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, response));
            }

            return response;
        }

		[HttpPost]
		[Route("file/upload")]
		public Response FileUpload()
		{
			// TODO: написать получение файла тут
			throw new NotImplementedException();
		}

        [NonAction]
        private string GetTokenValue()
        {
            return Request.Headers.GetValues("Token").FirstOrDefault();
        }
    }
}
