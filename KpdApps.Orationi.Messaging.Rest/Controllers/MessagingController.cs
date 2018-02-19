using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KpdApps.Orationi.Messaging.Core;
using KpdApps.Orationi.Messaging.DataAccess;
using KpdApps.Orationi.Messaging.DataAccess.Models;
using KpdApps.Orationi.Messaging.Models;
using Microsoft.AspNetCore.Mvc;

namespace KpdApps.Orationi.Messaging.Rest.Controllers
{
    [Route("api/rest/[controller]")]
    public class MessagingController : Controller, IMessagingService
    {
        OrationiMessagingContext _dbContext;

        public MessagingController(OrationiMessagingContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("version")]
        public string GetVersion()
        {
            return "v.1.0";
        }

        [HttpGet("{requestId}")]
        public Response GetResponse(Guid requestId)
        {
            Message message = _dbContext.Messages.FirstOrDefault(m => m.Id == requestId);
            if (message == null)
            {
                return new Response() { Id = requestId, IsError = true, Error = $"Request {requestId} not found" };
            }

            return new Response() { Id = requestId, IsError = false, Error = null, ResponseBody = message.ResponseBody };
        }

        [HttpPost]
        public Response ExecuteRequest([FromBody] Request request)
        {
            //Если указали алиас типа запроса, но не указали код - получаем код запроса
            if (!string.IsNullOrEmpty(request.RequestType) && request.RequestCode == 0)
            {
                RequestCodeAlias requestCodeAlias = _dbContext.RequestCodeAliases.FirstOrDefault(rca => rca.Alias == request.RequestType);
                if (requestCodeAlias == null)
                {
                    return new Response() { Id = Guid.Empty, IsError = true, Error = "Invalid request type." };
                }
                request.RequestCode = requestCodeAlias.RequestCode;
            }

            // Отдаем запрос в процессор, дальше он сам
            IncomingMessageProcessor imp = new IncomingMessageProcessor(_dbContext);
            Response response = imp.Execute(request);
            return response;
        }

        [HttpPost("request")]
        public ResponseId SendRequest(Request request)
        {
            throw new NotImplementedException();
        }

        [HttpPost("async")]
        public ResponseId ExecuteRequestAsync(Request request)
        {
            IncomingMessageProcessor imp = new IncomingMessageProcessor(_dbContext);
            ResponseId response = imp.ExecuteAsync(request);
            return response;
        }
    }
}
