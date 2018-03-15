using KpdApps.Orationi.Messaging.DataAccess;
using KpdApps.Orationi.Messaging.DataAccess.Models;
using KpdApps.Orationi.Messaging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

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
                var response = new Response();

                if (!_httpContext.IsAuthorized(_dbContext, request.Code, response, out var externalSystem))
                    return response;

                SetRequestCode(request);
                var message = new Message
                {
                    RequestBody = request.Body,
                    RequestCode = request.Code,
                    RequestUser = request.UserName,
                    ExternalSystemId = externalSystem.ExternalSystemId,
                    IsSyncRequest = true
                };

                _dbContext.Messages.Attach(message);
                _dbContext.SaveChanges();

                var client = new RabbitClient(request.Code, true);
                client.Execute(message.RequestCode, message.Id);

                message = _dbContext.Messages.FirstOrDefault(m => m.Id == message.Id);

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
            var response = new Response();
            var message = _dbContext.Messages.FirstOrDefault(m => m.Id == requestId);
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
            return !_httpContext.IsAuthorized(_dbContext, message.RequestCode, response, out var externalSystem)
                ? response
                : new Response() {Id = requestId, IsError = false, Error = null, Body = message.ResponseBody};
        }

        public ResponseId ExecuteAsync(Request request)
        {
            try
            {
                var response = new ResponseId();

                if (!_httpContext.IsAuthorized(_dbContext, request.Code, response, out var externalSystem))
                    return response;

                SetRequestCode(request);
                var message = new Message
                {
                    RequestBody = request.Body,
                    RequestCode = request.Code,
                    RequestUser = request.UserName,
                    ExternalSystemId = externalSystem.ExternalSystemId,
                    IsSyncRequest = false
                };

                _dbContext.Messages.Add(message);
                _dbContext.SaveChanges();

                var client = new RabbitClient(request.Code, false);
                client.PullMessage(message.RequestCode, message.Id);

                response.Id = message.Id;

                return response;
            }
            catch (Exception ex)
            {
                return new ResponseId() { IsError = true, Error = $"{ex.Message} {(ex.InnerException is null ? "" : ex.InnerException.Message)}" };
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
