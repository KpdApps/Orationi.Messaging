using KpdApps.Orationi.Messaging.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KpdApps.Orationi.Messaging
{
    [Route("api/rest/[controller]")]
    public class MessagingController : Controller, IMessagingService
    {
        [HttpPost("execute")]
        public Response ExecuteRequest(Request request)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{requestId}")]
        public Response GetResponse(Guid requestId)
        {
            throw new NotImplementedException();
        }

        [HttpPost("request")]
        public ResponseId SendRequest(Request request)
        {
            throw new NotImplementedException();
        }

        [HttpPost("requestasync")]
        public ResponseId SendRequestAsync(Request request)
        {
            throw new NotImplementedException();
        }
    }
}
