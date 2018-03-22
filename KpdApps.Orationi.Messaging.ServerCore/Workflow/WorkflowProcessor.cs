using KpdApps.Orationi.Messaging.DataAccess;
using KpdApps.Orationi.Messaging.DataAccess.Models;
using KpdApps.Orationi.Messaging.ServerCore.Helpers;
using KpdApps.Orationi.Messaging.ServerCore.Pipeline;
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
            try
            {
                using (OrationiMessagingContext dbContext = new OrationiMessagingContext(ContextOptionsBuilderExtensions.GetContextOptionsBuilder()))
                {
                    _message = dbContext.Messages.FirstOrDefault(m => m.Id == _messageId);
                    _message.AttemptCount++;
                    _message.StatusCode = (int)WorkflowStatusCodes.Preparing;
                    dbContext.SaveChanges();

                    LoadWorkflowActions();

                    List<GlobalSetting> globalSettings = dbContext.GlobalSettings.ToList();
                    _workflowExecutionContext = new WorkflowExecutionContext(_message, globalSettings);
                }

                SetMessageStatus(WorkflowStatusCodes.InProgress);
                foreach (WorkflowAction workflowAction in _workflowActions)
                {
                    PipelineExecutionContext pipelineExecutionContext = new PipelineExecutionContext(_workflowExecutionContext);
                    PipelineProcessor pipeline = new PipelineProcessor(pipelineExecutionContext, workflowAction);
                    pipeline.Run();
                    //TODO: Temp solution
                    _message.ResponseBody = pipelineExecutionContext.ResponseBody;
                }
                SetMessageStatus(WorkflowStatusCodes.Processed);
            }
            catch (Exception ex)
            {
                _message.ErrorMessage = ex.Message;
                SetMessageStatus(WorkflowStatusCodes.Error);
            }
        }

        private void SetMessageStatus(WorkflowStatusCodes statusCode)
        {
            using (OrationiMessagingContext dbContext = new OrationiMessagingContext(ContextOptionsBuilderExtensions.GetContextOptionsBuilder()))
            {
                _message.StatusCode = (int)statusCode;
                dbContext.SaveChanges();
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
                                    orderby w.Id, wa.Order
                                    select new WorkflowAction
                                    {
                                        WorkflowId = w.Id,
                                        PluginActionSetId = wa.PluginActionSetId,
                                        Order = wa.Order,
                                    }
                                   ).ToList();
            }
        }
    }
}
