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
using log4net;
using log4net.Config;
using Newtonsoft.Json;

namespace KpdApps.Orationi.Messaging.Rest.Controllers
{
    [RoutePrefix("api/rest/messaging")]
    public class MessagingController : ApiController, IMessagingService
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(MessagingController));

        private OrationiDatabaseContext _dbContext;

        public MessagingController()
        {
            XmlConfigurator.Configure();
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
            log.Debug("Запуск");
            log.Debug($"requestId: {requestId}");
            log.Debug($"Token: {GetTokenValue()}");
            if (!AuthorizeHelpers.IsAuthorized(_dbContext, GetTokenValue(), requestId, out Response response, out var externalSystem))
            {
                log.Error($"Авторизация не пройдена. Причина: {response.Error}");
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Forbidden, response));
            }

            log.Debug("Авторизация пройдена");
            IncomingMessageProcessor imp = new IncomingMessageProcessor(_dbContext, externalSystem);
            response = imp.GetResponse(requestId);
            log.Debug($"Результат:\r\n{response}");
            log.Debug("Звершение");
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
            log.Debug("Запуск");
            log.Debug($"request:\r\n{request}");
            log.Debug($"Token: {GetTokenValue()}");
            if (!AuthorizeHelpers.IsAuthorized(_dbContext, GetTokenValue(), request.Code, out Response response, out var externalSystem))
            {
                log.Error($"Авторизация не пройдена. Причина: {response.Error}");
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Forbidden, response));
            }

            log.Debug("Авторизация пройдена");
            IncomingMessageProcessor imp = new IncomingMessageProcessor(_dbContext, externalSystem);
            response = imp.Execute(request);
            log.Debug($"Результат:\r\n{response}");
            log.Debug("Звершение");
            return response;
        }

        [HttpPost]
        [Route("async")]
        public ResponseId ExecuteRequestAsync([FromBody]Request request)
        {
            log.Debug("Запуск");
            log.Debug($"request:\r\n{request}");
            log.Debug($"Token: {GetTokenValue()}");
            if (!AuthorizeHelpers.IsAuthorized(_dbContext, GetTokenValue(), request.Code, out ResponseId response, out var externalSystem))
            {
                log.Error($"Авторизация не пройдена. Причина: {response.Error}");
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Forbidden, response));
            }

            log.Debug("Авторизация пройдена");
            IncomingMessageProcessor imp = new IncomingMessageProcessor(_dbContext, externalSystem);
            response = imp.ExecuteAsync(request);
            log.Debug($"Результат:\r\n{response}");
            log.Debug("Звершение");
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
            log.Debug("Запуск");
            log.Debug($"requestCode: {requestCode}");
            log.Debug($"Token: {GetTokenValue()}");
            if (!AuthorizeHelpers.IsAuthorized(_dbContext, GetTokenValue(), requestCode, out ResponseXsd response, out var externalSystem))
            {
                log.Error($"Авторизация не пройдена. Причина: {response.Error}");
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Forbidden, response));
            }

            log.Debug("Авторизация пройдена");
            var imp = new IncomingMessageProcessor(_dbContext, externalSystem);
            response = imp.GetXsd(requestCode);
            log.Debug($"Результат:\r\n{response}");

            if (response.IsError)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, response));
            }

            log.Debug("Звершение");
            return response;
        }

        [HttpPost]
        [Route("file/upload")]
        public async Task<Response> FileUpload()
        {
            log.Debug("Запуск");
            log.Debug($"Token: {GetTokenValue()}");

            var isMimeMultipartContent = Request.Content.IsMimeMultipartContent();
            log.Debug($"isMimeMultipartContent: {isMimeMultipartContent}");

            if (!isMimeMultipartContent)
            {
                var errorResponse = new ResponseId
                {
                    IsError = true,
                    Error = "Некорректный Content-Type, ожидаем на вход MimeMultipart"
                };
                log.Error(errorResponse);

                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.UnsupportedMediaType, errorResponse));
            }

            var provider = new MultipartMemoryStreamProvider();

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, new ResponseId
                {
                    IsError = true,
                    Error = ex.Message
                }));
            }

            log.Debug($"Contents count: {provider.Contents.Count}");

            if (provider.Contents.Count != 2)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, new ResponseId
                {
                    IsError = true,
                    Error = "Тело запроса должно состоять из файла и json-объекта"
                }));
            }

            HttpContent json = provider.Contents.FirstOrDefault(c => c.Headers.ContentType.MediaType == "application/json");
            if (json == null)
            {
                var errorResponse = new Response
                {
                    IsError = true,
                    Error = "В теле запроса нет части с Content-type: application/json"
                };
                log.Error(errorResponse);

                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse));
            }

            UploadFileRequest fileInfo = null;
            byte[] jsonAsArray = json.ReadAsByteArrayAsync().Result;
            using (var stream = new MemoryStream(jsonAsArray))
            {
                var sr = new StreamReader(stream);
                fileInfo = JsonConvert.DeserializeObject<UploadFileRequest>(sr.ReadToEnd());
            }
            log.Debug($"request: {fileInfo}");

            if (!AuthorizeHelpers.IsAuthorized(_dbContext, GetTokenValue(), fileInfo.RequestCode, out Response response,
                out var externalSystem))
            {
                log.Error($"Авторизация не пройдена. Причина: {response.Error}");
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Forbidden, response));
            }

            log.Debug("Авторизация пройдена");
            HttpContent file = provider.Contents.FirstOrDefault(c => c.Headers.ContentType.MediaType != "application/json");
            if (file == null)
            {
                var errorResponse = new ResponseId
                {
                    IsError = true,
                    Error = "В теле запроса нет части с файлом"
                };
                log.Error(errorResponse);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse));
            }

            string fileName = file.Headers.ContentDisposition.FileName.Replace("\"", "");
            log.Debug($"fileName: {fileName}");
            UploadFileRequest.ValidateFileName(fileName, Request);


            byte[] fileAsArray = file.ReadAsByteArrayAsync().Result;

            var imp = new IncomingMessageProcessor(_dbContext, externalSystem);
            response = imp.FileUpload(fileInfo, fileName, fileAsArray);
            log.Debug($"Результат:\r\n{response}");
            log.Debug("Завершение");
            return response;
        }

        [NonAction]
        private string GetTokenValue()
        {
            return Request.Headers.GetValues("Token").FirstOrDefault();
        }
    }
}
