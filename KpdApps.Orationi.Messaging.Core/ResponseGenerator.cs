using System;
using System.Linq;
using KpdApps.Orationi.Messaging.Common.Models;
using KpdApps.Orationi.Messaging.DataAccess;
using KpdApps.Orationi.Messaging.DataAccess.Models;
using KpdApps.Orationi.Messaging.ServerCore.Workflow;

namespace KpdApps.Orationi.Messaging.Core
{
    public class ResponseGenerator
    {
        private readonly OrationiDatabaseContext _dbContext;
        private readonly Guid _requestId;

        public ResponseGenerator(OrationiDatabaseContext context, Guid requestId)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            _dbContext = context;
            _requestId = requestId;
        }

        public Response GenerateByMessageAndRequestId()
        {
            Message message = _dbContext.Messages.FirstOrDefault(m => m.Id == _requestId);

            if (message is null)
                return GetNullResponse(_requestId);

            if (message.StatusCode == (int)MessageStatusCodes.Error)
                return GetErrorResponse(message);

            return new Response
            {
                Id = message.Id,
                Body = message.ResponseBody
            };
        }

        private Response GetNullResponse(Guid requestId)
        {
            return new Response
            {
                Id = requestId,
                IsError = true,
                Error = $"Запрос {requestId} не найден!"
            };
        }

        private Response GetErrorResponse(Message message)
        {
            var resultResponse = new Response
            {
                Id = message.Id,
                IsError = true
            };

            ProcessingError processingError = _dbContext.ProcessingErrors.FirstOrDefault(pe => pe.MessageId == message.Id);
            if (processingError != null)
            {
                resultResponse.Error = processingError.Error;
            }

            return resultResponse;
        }

        public static Response GenerateByException(Exception exception)
        {
            return new Response
            {
                IsError = true,
                Error = $"{exception.Message}, inner: {(exception.InnerException is null ? "no inner" : exception.InnerException.Message)}"
            };
        }
    }
}