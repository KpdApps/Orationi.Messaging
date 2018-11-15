using KpdApps.Orationi.Messaging.DataAccess;
using KpdApps.Orationi.Messaging.Sdk;
using KpdApps.Orationi.Messaging.ServerCore.Workflow;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KpdApps.Orationi.Messaging.DataAccess.Models;
using KpdApps.Orationi.Messaging.Sdk.Cache;
using KpdApps.Orationi.Messaging.ServerCore.Cache;

namespace KpdApps.Orationi.Messaging.ServerCore.Pipeline
{
    internal class PipelineExecutionContext : IPipelineExecutionContext
    {
        public string RequestBody { get; set; }

        public string ResponseBody { get; set; }

        public string ResponseUser { get; set; }

        public string ResponseSystem { get; set; }

        public int? StatusCode { get; set; }

        public IDictionary PipelineValues { get; }

        public IDictionary PluginStepSettings { get; set; }

        public Guid MessageId => _workflowExecutionContext.MessageId;

        public int RequestCode => _workflowExecutionContext.RequestCode;

        public string MessageBody => _workflowExecutionContext.MessageBody;

        public IDictionary GlobalSettings => _workflowExecutionContext.GlobalSettings;

        private IWorkflowExecutionContext _workflowExecutionContext;

        private readonly OrationiDatabaseContext _dbContext;

        public ICacheProvider CacheProvider { get; }

        internal PipelineExecutionContext(IWorkflowExecutionContext workflowExecutionContext, OrationiDatabaseContext dbContext)
        {
            PipelineValues = new Dictionary<string, object>();
            PluginStepSettings = new Dictionary<string, object>();
            _workflowExecutionContext = workflowExecutionContext;
            _dbContext = dbContext;
            CacheProvider = CacheProviderFactory.Create(dbContext);
            RequestBody = _workflowExecutionContext.MessageBody;
        }

        public byte[] GetFile(Guid messageId, out string filename)
        {
            var fileStore = _dbContext.FileStores.FirstOrDefault(f => f.MessageId == messageId);

            if (fileStore == null)
                throw new ArgumentNullException($"Не нашли файл, связанный с сообщением {messageId}");

            filename = fileStore.FileName;

            // Массив - ссылочный тип, не хотим давать ссылку на объект в БД.
            return fileStore.Data.ToArray();
        }

        public CallbackSettings TryGetCallbackSettings(Guid messageId, out int? requestCode)
        {
            var message = _dbContext.Messages.FirstOrDefault(m => m.Id == messageId);
            requestCode = message?.RequestCodeId;
            return message?.ExternalSystem?.CallbackSettings;
        }
    }
}