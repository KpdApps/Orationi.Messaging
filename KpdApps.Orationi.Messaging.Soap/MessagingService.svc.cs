using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading;
using KpdApps.Orationi.Messaging.Common.Models;
using KpdApps.Orationi.Messaging.Core;
using KpdApps.Orationi.Messaging.DataAccess;
using log4net;
using log4net.Config;

namespace KpdApps.Orationi.Messaging.Soap
{
    public class MessagingService : IMessagingService
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(MessagingService));
        private readonly OrationiDatabaseContext _dbContext;

        public MessagingService()
        {
            _dbContext = new OrationiDatabaseContext();
            XmlConfigurator.Configure();
        }

        public Response GetStatus(Guid requestId)
        {
            throw new NotImplementedException();
        }

        public Response ExecuteRequest(Request request)
        {
            log.Debug("Запуск");
            log.Debug($"Сообщение:\r\n{OperationContext.Current.RequestContext.RequestMessage}");
            log.Debug($"request:\r\n{request}");
            log.Debug($"Token: {WebOperationContext.Current.IncomingRequest.Headers["Token"]}");
            if (!AuthorizeHelpers.IsAuthorized(
                _dbContext,
                WebOperationContext.Current.IncomingRequest.Headers["Token"],
                request.Code,
                out Response response,
                out var externalSystem))
            {
                log.Error($"Авторизация не пройдена. Причина: {response.Error}");
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Forbidden;
                return response;
            }

            log.Debug("Авторизация пройдена");
            IncomingMessageProcessor imp = new IncomingMessageProcessor(_dbContext, externalSystem);
            response = imp.Execute(request);
            log.Debug($"Результат:\r\n{response}");
            log.Debug("Звершение");
            return response;
        }

        public Response GetResponse(Guid requestId)
        {
            log.Debug("Запуск");
            log.Debug($"Сообщение:\r\n{OperationContext.Current.RequestContext.RequestMessage}");
            log.Debug($"requestId: {requestId}");
            log.Debug($"Token: {WebOperationContext.Current.IncomingRequest.Headers["Token"]}");
            if (!AuthorizeHelpers.IsAuthorized(_dbContext,
                WebOperationContext.Current.IncomingRequest.Headers["Token"],
                requestId,
                out Response response,
                out var externalSystem))
            {
                log.Error($"Авторизация не пройдена. Причина: {response.Error}");
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Forbidden;
                return response;
            }

            log.Debug("Авторизация пройдена");
            IncomingMessageProcessor imp = new IncomingMessageProcessor(_dbContext, externalSystem);
            response = imp.GetResponse(requestId);
            log.Debug($"Результат:\r\n{response}");
            log.Debug("Звершение");
            return response;
        }

        public ResponseId SendRequest(Request request)
        {
            throw new NotImplementedException();
        }

        public ResponseId ExecuteRequestAsync(Request request)
        {
            log.Debug("Запуск");
            log.Debug($"Сообщение:\r\n{OperationContext.Current.RequestContext.RequestMessage}");
            log.Debug($"request:\r\n{request}");
            log.Debug($"Token: {WebOperationContext.Current.IncomingRequest.Headers["Token"]}");
            if (!AuthorizeHelpers.IsAuthorized(_dbContext,
                WebOperationContext.Current.IncomingRequest.Headers["Token"],
                request.Code,
                out ResponseId response,
                out var externalSystem))
            {
                log.Error($"Авторизация не пройдена. Причина: {response.Error}");
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Forbidden;
                return response;
            }

            log.Debug("Авторизация пройдена");
            IncomingMessageProcessor imp = new IncomingMessageProcessor(_dbContext, externalSystem);
            response = imp.ExecuteAsync(request);
            log.Debug($"Результат:\r\n{response}");
            log.Debug("Звершение");
            return response;
        }

        public string GetVersion()
        {
            return "v.1.0";
        }

        public ResponseXsd GetXsd(int requestCode)
        {
            log.Debug("Запуск");
            log.Debug($"Сообщение:\r\n{OperationContext.Current.RequestContext.RequestMessage}");
            log.Debug($"requestCode: {requestCode}");
            log.Debug($"Token: {WebOperationContext.Current.IncomingRequest.Headers["Token"]}");
            if (!AuthorizeHelpers.IsAuthorized(
                _dbContext,
                WebOperationContext.Current.IncomingRequest.Headers["Token"],
                requestCode,
                out ResponseXsd response,
                out var externalSystem))
            {
                log.Error($"Авторизация не пройдена. Причина: {response.Error}");
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Forbidden;
                return response;
            }

            log.Debug("Авторизация пройдена");
            IncomingMessageProcessor imp = new IncomingMessageProcessor(_dbContext, externalSystem);
            response = imp.GetXsd(requestCode);
            log.Debug($"Результат:\r\n{response}");

            if (response.IsError)
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }

            log.Debug("Звершение");
            return response;
        }
    }
}
