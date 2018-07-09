using KpdApps.Orationi.Messaging.ServerCore.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using KpdApps.Orationi.Messaging.DataAccess;
using KpdApps.Orationi.Messaging.DataAccess.Models;
using log4net;

namespace KpdApps.Orationi.Messaging.ServerCore.Workflow
{
    public class WorkflowProcessor : IDisposable
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(WorkflowProcessor));

        private Guid _messageId;
        private int _requestCode;
        private Message _message;
        private WorkflowExecutionContext _workflowExecutionContext;
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
                 _message = _dbContext.Messages.First(m => m.Id == _messageId);
                 _message.AttemptCount++;
                 _message.StatusCode = (int)MessageStatusCodes.Preparing;
                _dbContext.SaveChanges();

                var workflowActions = _dbContext
                    .WorkflowActions
                    .Where(wa => wa.Workflow.RequestCodeId == _requestCode)
                    .ToList();

                 List<GlobalSetting> globalSettings = _dbContext.GlobalSettings.ToList();
                 _workflowExecutionContext = new WorkflowExecutionContext(_message, globalSettings);

                SetMessageStatus(MessageStatusCodes.InProgress);
                foreach (var workflowAction in workflowActions)
                {
                    PipelineExecutionContext pipelineExecutionContext = new PipelineExecutionContext(_workflowExecutionContext, _dbContext);
                    PipelineProcessor pipeline = new PipelineProcessor(pipelineExecutionContext, workflowAction);
                    pipeline.Run();
                    //TODO: Temp solution
                    _message.ResponseBody = pipelineExecutionContext.ResponseBody;
                }
                SetMessageStatus(MessageStatusCodes.Processed);
            }
            catch (Exception ex)
            {
                log.Fatal("Во время выполнения работы WorkflowProcessor произошла ошибка", ex);
                _message.ErrorMessage = ex.Message;
                SetMessageStatus(MessageStatusCodes.Error);
            }
        }

        private void SetMessageStatus(MessageStatusCodes statusCode)
        {
            _message.StatusCode = (int)statusCode;
            _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }
    }
}
