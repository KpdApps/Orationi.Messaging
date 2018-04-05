using System;
using System.Linq;
using System.Web;
using KpdApps.Orationi.Messaging.Common.Models;
using KpdApps.Orationi.Messaging.DataAccess;
using KpdApps.Orationi.Messaging.DataAccess.Models;

namespace KpdApps.Orationi.Messaging.Core
{
    public class IncomingMessageProcessor
    {
        private readonly OrationiDatabaseContext _dbContext;
        private readonly ExternalSystem _externalSystem;

        public IncomingMessageProcessor(OrationiDatabaseContext dbContext, ExternalSystem externalSystem)
        {
            _dbContext = dbContext;
            _externalSystem = externalSystem;
        }

        public Response Execute(Request request)
        {
            try
            {
                Response response = new Response();

                SetRequestCode(request);
                Message message = new Message
                {
                    RequestBody = request.Body,
                    RequestCodeId = request.Code,
                    ExternalSystemId = _externalSystem.Id,
                    RequestUser = request.UserName,
                    IsSyncRequest = true
                };

                _dbContext.Messages.Add(message);
                _dbContext.SaveChanges();

                RabbitClient client = new RabbitClient();
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
                response.Id = requestId;
                response.IsError = true;
                response.Error = $"Request {requestId} not found";
                return response;
            }

            //TODO: Обработка статуса сообщения, если еще не обработано возвращаем статус / ошибку
            return new Response() { Id = requestId, IsError = false, Error = null, Body = message.ResponseBody };
        }

        public ResponseId ExecuteAsync(Request request)
        {
            try
            {
                SetRequestCode(request);
                Message message = new Message
                {
                    RequestBody = request.Body,
                    RequestCodeId = request.Code,
                    ExternalSystemId = _externalSystem.Id,
                    RequestUser = request.UserName,
                    IsSyncRequest = false
                };

                _dbContext.Messages.Add(message);
                _dbContext.SaveChanges();

                RabbitClient client = new RabbitClient();
                client.PullMessage(message.RequestCodeId, message.Id);

                ResponseId response = new ResponseId();
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
