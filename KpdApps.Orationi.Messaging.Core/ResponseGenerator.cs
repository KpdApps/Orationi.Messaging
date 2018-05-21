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
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (requestId == null)
                throw new ArgumentNullException(nameof(requestId));

            _dbContext = context;
            _requestId = requestId;
        }

        public Response GenerateByMessageAndRequestId()
        {
            Message message = _dbContext.Messages.FirstOrDefault(m => m.Id == _requestId);

            if (message is null)
                return GetNullResponse(_requestId);

            if (message.StatusCode == (int)MessageStatusCodes.New)
                return GetNewResponse(message);

            if (message.StatusCode == (int)MessageStatusCodes.Preparing)
                return GetPreparingResponse(message);

            if (message.StatusCode == (int)MessageStatusCodes.InProgress)
                return GetInProgressResponse(message);

            if (message.StatusCode == (int)MessageStatusCodes.Processed)
                return GetInProcessedResponse(message);

            if (message.StatusCode == (int)MessageStatusCodes.Error)
                return GetErrorResponse(message);

            // If we are here then someone added new status to DB and forgot to process it
            throw new InvalidOperationException(
                $"Couldn't generate response for status {message.StatusCode} in message {message.Id}");
        }

        private Response GetNullResponse(Guid requestId)
        {
            return new Response()
            {
                Id = requestId,
                IsError = true,
                Error = $"Request {requestId} not found",
                Body = null
            };
        }

        private Response GetNewResponse(Message message)
        {
            return new Response()
            {
                Id = message.Id,
                IsError = false,
                Error = null,
                Body = $"New. Wait for queing."
            };
        }

        private Response GetPreparingResponse(Message message)
        {
            return new Response()
            {
                Id = message.Id,
                IsError = false,
                Error = null,
                Body = $"Preparing for processing."
            };
        }

        private Response GetInProgressResponse(Message message)
        {
            return new Response()
            {
                Id = message.Id,
                IsError = false,
                Error = null,
                Body = $"Processing."
            };
        }

        private Response GetInProcessedResponse(Message message)
        {
            return new Response()
            {
                Id = message.Id,
                IsError = false,
                Error = null,
                Body = message.ResponseBody
            };
        }

        private Response GetErrorResponse(Message message)
        {
            var resultResponse = new Response()
            {
                Id = message.Id,
                IsError = true,
                Body = null
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
            return new Response()
            {
                IsError = true,
                Error = $"{exception.Message}, inner: {(exception.InnerException is null ? "no inner" : exception.InnerException.Message)}"
            };
        }
    }
}