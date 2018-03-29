using System;
using KpdApps.Orationi.Messaging.Common.Models;
using KpdApps.Orationi.Messaging.DataAccess.Common.Models;

namespace KpdApps.Orationi.Messaging.Core
{
    public class IncomingMessageProcessor
    {
        private readonly OrationiMessagingContext _dbContext;
        private readonly HttpContext _httpContext;

        public IncomingMessageProcessor(OrationiMessagingContext dbContext, HttpContext httpContext)
        {
            _dbContext = dbContext;
            _httpContext = httpContext;
        }

        public Response Execute(Request request)
        {
            try
            {
                Response response = new Response();

                if (!_httpContext.IsAuthorized(_dbContext, request.Code, response, out var externalSystem))
                    return response;

                SetRequestCode(request);
                Message message = new Message
                {
                    RequestBody = request.Body,
                    RequestCodeId = request.Code,
                    ExternalSystemId = externalSystem.Id,
                    RequestUser = request.UserName,
                    IsSyncRequest = true
                };

                _dbContext.Messages.Attach(message);
                _dbContext.SaveChanges();

                RabbitClient client = new RabbitClient(request.Code, true);
                client.Execute(message.RequestCodeId, message.Id);

                message = _dbContext.Messages.FirstOrDefault(m => m.Id == message.Id);

                response = new Response();
                response.Id = message.Id;
                response.Body = message.ResponseBody;

                if (!string.IsNullOrEmpty(message.ErrorMessage))
                {
                    response.IsError = true;
                    response.Error = message.ErrorMessage;
                }

                return response;
            }
            catch (Exception ex)
            {
                return new Response() { IsError = true, Error = $"{ex.Message} {(ex.InnerException is null ? "" : ex.InnerException.Message)}" };
            }
        }

        public Response GetResponse(Guid requestId)
        {
            Response response = new Response();
            Message message = _dbContext.Messages.FirstOrDefault(m => m.Id == requestId);

            if (message is null)
            {
                _httpContext.Response.StatusCode = 400;
                response.Id = requestId;
                response.IsError = true;
                response.Error = $"Request {requestId} not found";
                return response;
            }

            //TODO: Обработка статуса сообщения, если еще не обработано возвращаем статус / ошибку
            //TODO: Вот действительно страннно, что система однозначно не может знать, что за RequestCode  будет по запрашиваемому идентификатору, будет нежданчик
            return !_httpContext.IsAuthorized(_dbContext, message.RequestCodeId, response, out var externalSystem)
                ? response
                : new Response() { Id = requestId, IsError = false, Error = null, Body = message.ResponseBody };
        }

        public ResponseId ExecuteAsync(Request request)
        {
            try
            {
                ResponseId response = new ResponseId();

                if (!_httpContext.IsAuthorized(_dbContext, request.Code, response, out var externalSystem))
                    return response;
                SetRequestCode(request);
                Message message = new Message
                {
                    RequestBody = request.Body,
                    RequestCodeId = request.Code,
                    ExternalSystemId = externalSystem.Id,
                    RequestUser = request.UserName,
                    IsSyncRequest = false
                };

                _dbContext.Messages.Attach(message);
                _dbContext.SaveChanges();

                RabbitClient client = new RabbitClient(request.Code, false);
                client.PullMessage(message.RequestCodeId, message.Id);

                response = new ResponseId();
                response.Id = message.Id;

                return response;
            }
            catch (Exception ex)
            {
                return new Response() { IsError = true, Error = $"{ex.Message} {(ex.InnerException is null ? "" : ex.InnerException.Message)}" };
            }
        }

        public void SetRequestCode(Request request)
        {
            //Если указали алиас типа запроса, но не указали код - получаем код запроса
            if (!string.IsNullOrEmpty(request.Type) && request.Code == 0)
            {
                RequestCodeAlias requestCodeAlias = _dbContext.RequestCodeAliases.FirstOrDefault(rca => rca.Alias == request.Type);
                if (requestCodeAlias == null)
                {
                    throw new InvalidOperationException("Invalid request type.");
                }
                request.Code = requestCodeAlias.RequestCode;
            }
        }
    }
}
