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
        public Common.Models.Response GetResponse(Guid requestId)
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
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Forbidden, response));
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
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Forbidden, response));
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
        public Common.Models.ResponseXsd GetXsd(int requestCode)
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

            var request = _dbContext
                .RequestCodes
                .FirstOrDefault(rc => rc.Id == requestCode);

            if (request is null)
            {
                response.IsError = true;
                response.Error = $"Для requestCode = {requestCode}, отсутствует запись в БД";
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, response));
            }

            var registeredPlugin = request
                .Workflows
                .FirstOrDefault()
                ?.WorkflowActions
                .OrderBy(wfa => wfa.Order)
                .FirstOrDefault()
                ?.PluginActionSet
                .PluginActionSetItems
                .OrderBy(pasi => pasi.Order)
                .FirstOrDefault()
                ?.RegisteredPlugin;

            if (registeredPlugin is null)
            {
                response.IsError = true;
                response.Error = $"Для requestCode = {requestCode}, не найден подходящий зарегистрированный плагин.{Environment.NewLine}Порядок поиска Workflows (1) — WorkflowActions (сортировка, 1) — PluginActionSet — PluginActionSetItems (сортировка, 1) — RegisteredPlugin";
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, response));
            }

            var assembly = Assembly.Load(registeredPlugin
                .PluginAssembly
                .Assembly);

            var pluginType = assembly.GetType(registeredPlugin
                .Class);

            response.RequestContractUri =
                ((ContractAttribute) pluginType.GetCustomAttribute(typeof(RequestContractAttribute)))
                ?.GetXsd(assembly);
            response.ResponseContractUri =
                ((ContractAttribute) pluginType.GetCustomAttribute(typeof(ResponseContractAttribute)))
                ?.GetXsd(assembly);

            return response;
        }

        [NonAction]
        private string GetTokenValue()
        {
            return Request.Headers.GetValues("Token").FirstOrDefault();
        }
    }
}
