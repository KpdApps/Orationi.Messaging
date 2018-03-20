using KpdApps.Orationi.Messaging.DataAccess;
using KpdApps.Orationi.Messaging.DataAccess.Models;
using KpdApps.Orationi.Messaging.ServerCore.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KpdApps.Orationi.Messaging.ServerCore.Workflow
{
    public class WorkflowProcessor
    {
        private Guid _messageId;
        private int _requestCode;

        private Message _message;

        private WorkflowExecutionContext _workflowExecutionContext;

        private List<WorkflowAction> _workflowActions;

        public WorkflowProcessor(Guid messageId, int requestCode)
        {
            _messageId = messageId;
            _requestCode = requestCode;
        }

        public void Run()
        {
            using (OrationiMessagingContext dbContext = new OrationiMessagingContext(ContextOptionsBuilderExtensions.GetContextOptionsBuilder()))
            {
                _message = dbContext.Messages.FirstOrDefault(m => m.Id == _messageId);
                _message.AttemptCount++;
                dbContext.SaveChanges();

                LoadWorkflowActions();

                _workflowExecutionContext = new WorkflowExecutionContext(_messageId, _requestCode, _message.RequestBody);

                var globalSettings = dbContext.GlobalSettings.ToList();

                globalSettings.ForEach(globalSetting =>
                {
                    _workflowExecutionContext.GlobalSettings.Add(globalSetting.Name, globalSetting.Value);
                });
            }

            foreach (WorkflowAction workflowAction in _workflowActions)
            {
                Pipeline.Pipeline pipeline = new Pipeline.Pipeline(_workflowExecutionContext, workflowAction.PluginActionSetId);
                pipeline.Run();
            }
        }

        public void LoadWorkflowActions()
        {
            using (OrationiMessagingContext dbContext = new OrationiMessagingContext(ContextOptionsBuilderExtensions.GetContextOptionsBuilder()))
            {
                _workflowActions = (from w in dbContext.Workflows
                                    join wa in dbContext.WorkflowActions
                                         on w.Id equals wa.WorkflowId
                                    where
                                         w.RequestCodeId == _requestCode
                                    orderby wa.Order
                                    select new WorkflowAction
                                    {
                                        PluginActionSetId = wa.PluginActionSetId,
                                        Order = wa.Order
                                    }
                                   ).ToList();
            }
        }
    }
}
