using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        [HttpGet]
        public string Index()
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
            try
            {
                int requestCode = 0;
                if (!string.IsNullOrEmpty(request.RequestType))
                {
                    RequestCodeAlias requestCodeAlias = _dbContext.RequestCodeAliases.FirstOrDefault(rca => rca.Alias == request.RequestType);
                    requestCode = requestCodeAlias.RequestCode;
                }
                requestCode = request.RequestCode;

                Message message = new Message
                {
                    RequestBody = request.RequestBody,
                    RequestCode = requestCode,
                    RequestSystem = request.RequestSystemName,
                    RequestUser = request.RequestUserName,
                    IsSyncRequest = true
                };

                _dbContext.Messages.Attach(message);
                _dbContext.SaveChanges();

                return new Response() { Id = message.Id };
            }
            catch (Exception ex)
            {
                return new Response() { IsError = true, Error = ex.Message };
            }
        }

        [HttpPost("request")]
        public ResponseId SendRequest(Request request)
        {
            try
            {
                int requestCode = 0;
                if (!string.IsNullOrEmpty(request.RequestType))
                {
                    RequestCodeAlias requestCodeAlias = _dbContext.RequestCodeAliases.FirstOrDefault(rca => rca.Alias == request.RequestType);
                    requestCode = requestCodeAlias.RequestCode;
                }
                requestCode = request.RequestCode;

                Message message = new Message
                {
                    RequestBody = request.RequestBody,
                    RequestCode = requestCode,
                    RequestSystem = request.RequestSystemName,
                    RequestUser = request.RequestUserName,
                    IsSyncRequest = true
                };

                _dbContext.Messages.Attach(message);
                _dbContext.SaveChanges();

                return new Response() { Id = message.Id };
            }
            catch (Exception ex)
            {
                return new Response() { IsError = true, Error = ex.Message };
            }
        }

        [HttpPost("async")]
        public Response ExecuteRequestAsync(Request request)
        {
            try
            {
                int requestCode = 0;
                if (!string.IsNullOrEmpty(request.RequestType))
                {
                    RequestCodeAlias requestCodeAlias = _dbContext.RequestCodeAliases.FirstOrDefault(rca => rca.Alias == request.RequestType);
                    requestCode = requestCodeAlias.RequestCode;
                }
                requestCode = request.RequestCode;

                Message message = new Message
                {
                    RequestBody = request.RequestBody,
                    RequestCode = requestCode,
                    RequestSystem = request.RequestSystemName,
                    RequestUser = request.RequestUserName,
                    IsSyncRequest = true
                };

                _dbContext.Messages.Attach(message);
                _dbContext.SaveChanges();

                return new Response() { Id = message.Id };
            }
            catch (Exception ex)
            {
                return new Response() { IsError = true, Error = ex.Message };
            }
        }
    }
}
