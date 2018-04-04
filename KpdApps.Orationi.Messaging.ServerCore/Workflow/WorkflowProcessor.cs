using KpdApps.Orationi.Messaging.ServerCore.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using KpdApps.Orationi.Messaging.DataAccess;
using KpdApps.Orationi.Messaging.DataAccess.Models;

namespace KpdApps.Orationi.Messaging.ServerCore.Workflow
{
    public class WorkflowProcessor : IDisposable
    {
        private Guid _messageId;
        private int _requestCode;

        private Message _message;

        private WorkflowExecutionContext _workflowExecutionContext;

        private List<WorkflowAction> _workflowActions;

        private OrationiDatabaseContext _dbContext;

        public WorkflowProcessor(Guid messageId, int requestCode)
        {
            _messageId = messageId;
            _requestCode = requestCode;
            _dbContext = new OrationiDatabaseContext();
        }

        public void Run()
        {
            try
            {
                 _message = _dbContext.Messages.FirstOrDefault(m => m.Id == _messageId);
                 _message.AttemptCount++;
                 _message.StatusCode = (int)MessageStatusCodes.Preparing;
                _dbContext.SaveChanges();

                 LoadWorkflowActions();

                 List<GlobalSetting> globalSettings = _dbContext.GlobalSettings.ToList();
                 _workflowExecutionContext = new WorkflowExecutionContext(_message, globalSettings);

                SetMessageStatus(MessageStatusCodes.InProgress);
                foreach (WorkflowAction workflowAction in _workflowActions)
                {
                    PipelineExecutionContext pipelineExecutionContext = new PipelineExecutionContext(_workflowExecutionContext);
                    PipelineProcessor pipeline = new PipelineProcessor(pipelineExecutionContext, workflowAction);
                    pipeline.Run();
                    //TODO: Temp solution
                    _message.ResponseBody = pipelineExecutionContext.ResponseBody;
                }
                SetMessageStatus(MessageStatusCodes.Processed);
            }
            catch (Exception ex)
            {
                _message.ErrorMessage = ex.Message;
                SetMessageStatus(MessageStatusCodes.Error);
            }
        }

        private void SetMessageStatus(MessageStatusCodes statusCode)
        {
            _message.StatusCode = (int)statusCode;
            _dbContext.SaveChanges();
        }

        public void LoadWorkflowActions()
        {
            using (OrationiDatabaseContext dbContext = new OrationiDatabaseContext())
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

        public void Dispose()
        {
            _dbContext?.Dispose();
        }
    }
}
