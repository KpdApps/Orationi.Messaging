using System;
using System.Collections;

namespace KpdApps.Orationi.Messaging.ServerCore.Workflow
{
    public class WorkflowExecutionContext : IWorkflowExecutionContext
    {
        public Guid MessageId { get; set; }

        public int RequestCode { get; set; }

        public string MessageBody { get; set; }

        public IDictionary GlobalSettings { get; set; }

        public WorkflowExecutionContext(Guid messageId, int requestCode, string messageBody)
        {
            MessageId = messageId;

            RequestCode = requestCode;

            MessageBody = messageBody;
        }
    }
}
