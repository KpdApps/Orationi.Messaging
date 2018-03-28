using System;
using System.Collections;
using System.Collections.Generic;
using KpdApps.Orationi.Messaging.DataAccess.Common.Models;

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

            RequestCode = message.RequestCodeId;

            MessageBody = message.RequestBody;

            GlobalSettings = new Dictionary<string, object>();
            globalSettings.ForEach(globalSetting =>
            {
                GlobalSettings.Add(globalSetting.Name, globalSetting.Value);
            });
        }
    }
}
