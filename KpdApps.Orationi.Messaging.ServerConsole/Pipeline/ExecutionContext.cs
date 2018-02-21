﻿using KpdApps.Orationi.Messaging.Sdk.Plugins;
using System.Collections;
using System.Collections.Generic;

namespace KpdApps.Orationi.Messaging.ServerConsole.Pipeline
{
    internal class ExecuteContext : IExecuteContext
    {
        public string RequestBody { get; internal set; }

        public string ResponseBody { get; set; }

        public IDictionary PipelineValues { get; }

        internal ExecuteContext()
        {
            PipelineValues = new Dictionary<string, object>();
        }
    }
}
