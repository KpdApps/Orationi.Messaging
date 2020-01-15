using System;
using System.Linq;
using KpdApps.Orationi.Messaging.Common.Models;
using KpdApps.Orationi.Messaging.DataAccess;
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
            var message = _dbContext
                .Messages
                .Where(m => m.Id == _requestId)
                .Select(m => new { 
                    m.Id,
                    m.ResponseBody,
                    m.StatusCode,
                    m.ErrorMessage
                })
                .FirstOrDefault();

            if (message is null)
                return GetNullResponse(_requestId);

            if (message.StatusCode == (int)MessageStatusCodes.Error)
                return GetErrorResponse(message.Id, message.ErrorMessage);

            return new Response
            {
                Id = message.Id,
                Body = message.ResponseBody,
                IsError = false,
                Error = null
            };
        }

        private Response GetNullResponse(Guid requestId)
        {
            return new Response
            {
                Id = requestId,
                IsError = true,
                Error = $"Запрос {requestId} не найден!",
                Body = null
            };
        }

        private Response GetErrorResponse(Guid messageId, string errorMessage)
        {
            return new Response
            {
                Id = messageId,
                IsError = true,
                Body = null,
                Error = errorMessage
            };            
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