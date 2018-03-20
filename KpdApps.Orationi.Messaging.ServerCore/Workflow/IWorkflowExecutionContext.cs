using System;
using System.Collections;

namespace KpdApps.Orationi.Messaging.ServerCore.Workflow
{
    public interface IWorkflowExecutionContext
    {
        Guid MessageId { get; set; }

        int RequestCode { get; set; }

        string MessageBody { get; set; }

        IDictionary GlobalSettings { get; set; }
    }
}
