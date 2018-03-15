﻿using System;
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
            var imp = new IncomingMessageProcessor(_dbContext, HttpContext);
            var response = imp.GetResponse(requestId);
            return response;
        }

        [HttpGet("status/{requestId}")]
        public Response GetStatus(Guid requestId)
        {
            throw new NotImplementedException();
        }

        [HttpPost("sync")]
        public Response ExecuteRequest(Request request)
        {
            var imp = new IncomingMessageProcessor(_dbContext, HttpContext);
            var response = imp.Execute(request);
            return response;
        }

        [HttpPost("async")]
        public ResponseId ExecuteRequestAsync(Request request)
        {
            var imp = new IncomingMessageProcessor(_dbContext, HttpContext);
            var response = imp.ExecuteAsync(request);
            return response;
        }

        [HttpPost("request")]
        public ResponseId SendRequest(Request request)
        {
            throw new NotImplementedException();
        }

        [HttpGet("xsd/{requestCode}")]
        public Response GetXsd(int requestCode)
        {
            var response = new Response();
            if (!HttpContext.IsAuthorized(_dbContext, requestCode, response, out var externalSystem))
                return response;

            response.ResponseBody = "test";

            return response;
        }
    }
}
