using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using KpdApps.Orationi.Messaging.Common.Models;
using KpdApps.Orationi.Messaging.Core;
using KpdApps.Orationi.Messaging.DataAccess;
using Newtonsoft.Json;

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
            if (!AuthorizeHelpers.IsAuthorized(_dbContext, GetTokenValue(), requestId,out Response response, out var externalSystem))
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
        public Response ExecuteRequest([FromBody]Request request)
        {
            if (!AuthorizeHelpers.IsAuthorized(_dbContext, GetTokenValue(), request.Code, out Response response, out var externalSystem))
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Forbidden, response));
            }

            IncomingMessageProcessor imp = new IncomingMessageProcessor(_dbContext, externalSystem);
            response = imp.Execute(request);
            return response;
        }

        [HttpPost]
        [Route("async")]
        public ResponseId ExecuteRequestAsync([FromBody]Request request)
        {
            if (!AuthorizeHelpers.IsAuthorized(_dbContext, GetTokenValue(), request.Code, out ResponseId response, out var externalSystem))
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Forbidden, response));
            }

            IncomingMessageProcessor imp = new IncomingMessageProcessor(_dbContext, externalSystem);
            response = imp.ExecuteAsync(request);
            return response;
        }

        [HttpPost]
        [Route("request")]
        public ResponseId SendRequest([FromBody]Request request)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("xsd/{requestCode}")]
        public ResponseXsd GetXsd(int requestCode)
        {
            if (!AuthorizeHelpers.IsAuthorized(_dbContext, GetTokenValue(), requestCode, out ResponseXsd response, out var externalSystem))
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
        public async Task<Response> FileUpload()
        {
            var isMimeMultipartContent = Request.Content.IsMimeMultipartContent();

            if (!isMimeMultipartContent)
                throw new HttpResponseException(Request.CreateResponse(
                    HttpStatusCode.UnsupportedMediaType, 
                    new Response { IsError = true, Error = "Некорректный Content-Type, ожидаем на вход MimeMultipart" })
                );

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);

            if (provider.Contents.Count != 2)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            HttpContent json = provider.Contents.FirstOrDefault(c => c.Headers.ContentType.MediaType == "application/json");
            if (json == null)
                throw new HttpResponseException(Request.CreateResponse(
                    HttpStatusCode.BadRequest,
                    new Response { IsError = true, Error = "В теле запроса нет части с Content-type: application/json" })
                );

            UploadFileRequest fileInfo = null;
            byte[] jsonAsArray = json.ReadAsByteArrayAsync().Result;
            using (var stream = new MemoryStream(jsonAsArray))
            {
                var sr = new StreamReader(stream);
                fileInfo = JsonConvert.DeserializeObject<UploadFileRequest>(sr.ReadToEnd());
            }

            if (!AuthorizeHelpers.IsAuthorized(_dbContext, GetTokenValue(), fileInfo.RequsetCode, out Response response, out var externalSystem))
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Forbidden, response));

            HttpContent file = provider.Contents.FirstOrDefault(c => c.Headers.ContentType.MediaType != "application/json");
            if (file == null)
                throw new HttpResponseException(Request.CreateResponse(
                    HttpStatusCode.BadRequest,
                    new Response { IsError = true, Error = "В теле запроса нет части с файлом" })
                );

            string fileName = file.Headers.ContentDisposition.FileName.Replace("\"", "");
            UploadFileRequest.ValidateFileName(fileName, Request);


            byte[] fileAsArray = file.ReadAsByteArrayAsync().Result;

            var imp = new IncomingMessageProcessor(_dbContext, externalSystem);
            response = imp.FileUpload(fileInfo, fileName, fileAsArray);

            return response;
        }

        [NonAction]
        private string GetTokenValue()
        {
            return Request.Headers.GetValues("Token").FirstOrDefault();
        }
    }
}
