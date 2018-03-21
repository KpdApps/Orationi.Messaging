using KpdApps.Orationi.Messaging.DataAccess.Models;
using KpdApps.Orationi.Messaging.Sdk;
using System;
using System.Collections;
using System.Collections.Generic;

namespace KpdApps.Orationi.Messaging.ServerCore.Workflow
{
    public class WorkflowExecutionContext : IWorkflowExecutionContext
    {
        public Guid MessageId { get; private set; }

        public int RequestCode { get; private set; }

        public string MessageBody { get; private set; }

        public IDictionary GlobalSettings { get; private set; }

        public WorkflowExecutionContext(Message message, List<GlobalSetting> globalSettings)
        {
            MessageId = message.Id;

            RequestCode = message.RequestCode;

            MessageBody = message.RequestBody;

            GlobalSettings = new Dictionary<string, object>();
            globalSettings.ForEach(globalSetting =>
            {
                GlobalSettings.Add(globalSetting.Name, globalSetting.Value);
            });
        }
    }
}
