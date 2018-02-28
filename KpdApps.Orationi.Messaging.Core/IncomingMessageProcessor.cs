using KpdApps.Orationi.Messaging.DataAccess;
using KpdApps.Orationi.Messaging.DataAccess.Models;
using KpdApps.Orationi.Messaging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KpdApps.Orationi.Messaging.Core
{
    public class IncomingMessageProcessor
    {
        private OrationiMessagingContext _dbContext;

        public IncomingMessageProcessor(OrationiMessagingContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Response Execute(Request request)
        {
            try
            {
                SetRequestCode(request);
                Message message = new Message
                {
                    RequestBody = request.RequestBody,
                    RequestCode = request.RequestCode,
                    RequestSystem = request.RequestSystemName,
                    RequestUser = request.RequestUserName,
                    IsSyncRequest = true
                };

                _dbContext.Messages.Attach(message);
                _dbContext.SaveChanges();

                RabbitClient client = new RabbitClient(request.RequestCode, true);
                client.Execute(message.RequestCode, message.Id);

                message = _dbContext.Messages.FirstOrDefault(m => m.Id == message.Id);

                Response response = new Response();
                response.Id = message.Id;
                response.ResponseBody = message.ResponseBody;

                if (!string.IsNullOrEmpty(message.ErrorMessage))
                {
                    response.IsError = true;
                    response.Error = message.ErrorMessage;
                }

                return response;
            }
            catch (Exception ex)
            {
                return new Response() { IsError = true, Error = ex.Message };
            }
        }

        public Response GetResponse(Guid requestId)
        {
            Message message = _dbContext.Messages.FirstOrDefault(m => m.Id == requestId);
            if (message == null)
            {
                return new Response() { Id = requestId, IsError = true, Error = $"Request {requestId} not found" };
            }

            //TODO: Обработка статуса сообщения, если еще не обработано возвращаем статус / ошибку

            return new Response() { Id = requestId, IsError = false, Error = null, ResponseBody = message.ResponseBody };
        }

        public ResponseId ExecuteAsync(Request request)
        {
            try
            {
                SetRequestCode(request);
                Message message = new Message
                {
                    RequestBody = request.RequestBody,
                    RequestCode = request.RequestCode,
                    RequestSystem = request.RequestSystemName,
                    RequestUser = request.RequestUserName,
                    IsSyncRequest = true
                };

                _dbContext.Messages.Attach(message);
                _dbContext.SaveChanges();

                RabbitClient client = new RabbitClient(request.RequestCode, false);
                client.PullMessage(message.RequestCode, message.Id);

                ResponseId response = new ResponseId();
                response.Id = message.Id;

                return response;
            }
            catch (Exception ex)
            {
                return new ResponseId() { IsError = true, Error = ex.Message };
            }
        }

        public void SetRequestCode(Request request)
        {
            //Если указали алиас типа запроса, но не указали код - получаем код запроса
            if (!string.IsNullOrEmpty(request.RequestType) && request.RequestCode == 0)
            {
                RequestCodeAlias requestCodeAlias = _dbContext.RequestCodeAliases.FirstOrDefault(rca => rca.Alias == request.RequestType);
                if (requestCodeAlias == null)
                {
                    throw new InvalidOperationException("Invalid request type.");
                }
                request.RequestCode = requestCodeAlias.RequestCode;
            }
        }
    }
}
