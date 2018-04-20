﻿using KpdApps.Orationi.Messaging.DataAccess.Models;
using System;
using System.Collections;

namespace KpdApps.Orationi.Messaging.Sdk
{
    public interface IPipelineExecutionContext
    {
        string RequestBody { get; set; }

        string ResponseBody { get; set; }

        string ResponseUser { get; set; }

        string ResponseSystem { get; set; }

        Nullable<int> StatusCode { get; set; }

        IDictionary PipelineValues { get; }

        IDictionary PluginStepSettings { get; set; }

        Guid MessageId { get; }

        int RequestCode { get; }

        string MessageBody { get; }

        IDictionary GlobalSettings { get; }

		byte[] GetFile(Guid messageId, out string filename);
	}
}