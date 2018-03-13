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
            IncomingMessageProcessor imp = new IncomingMessageProcessor(_dbContext);
            Response response = imp.GetResponse(requestId);
            return response;
        }

        [HttpPost]
        public Response ExecuteRequest([FromBody] Request request)
        {
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

        [HttpGet("xsd/{requestCode}")]
        public IActionResult GetXsd(int requestCode)
        {
            string result = "test";
            return Content(result);
        }
    }
}
