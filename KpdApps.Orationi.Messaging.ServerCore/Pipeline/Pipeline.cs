using KpdApps.Orationi.Messaging.DataAccess;
using KpdApps.Orationi.Messaging.DataAccess.Models;
using KpdApps.Orationi.Messaging.Sdk.Plugins;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KpdApps.Orationi.Messaging.ServerCore.Helpers;
using KpdApps.Orationi.Messaging.ServerCore.Workflow;

namespace KpdApps.Orationi.Messaging.ServerCore.Pipeline
{
    public class Pipeline : IDisposable
    {
        private Guid _messageId;
        private int _requestCode;
        private Guid _pluginActionSetId;

        private IExecuteContext _context;
        private OrationiMessagingContext _dbContext;
        private List<PipelineStepDescription> _stepsDescriptions;
        private Message _message;

        private IWorkflowExecutionContext _workflowExecutionContext;

        public Pipeline(Guid messageId, int requestCode)
        {
            _messageId = messageId;
            _requestCode = requestCode;
            Init();
        }

        public Pipeline(IWorkflowExecutionContext workflowExecutionContext, Guid pluginActionSetId)
        {
            _messageId = workflowExecutionContext.MessageId;
            _requestCode = workflowExecutionContext.RequestCode;
            _workflowExecutionContext = workflowExecutionContext;
            _pluginActionSetId = pluginActionSetId;
        }

        ~Pipeline()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_dbContext != null)
            {
                _dbContext.Dispose();
            }
        }

        public void Init()
        {
            _dbContext = new OrationiMessagingContext(ContextOptionsBuilderExtensions.GetContextOptionsBuilder());
            _message = _dbContext.Messages.FirstOrDefault(m => m.Id == _messageId);
            _message.AttemptCount++;
            _dbContext.SaveChanges();

            _context = new ExecuteContext
            {
                RequestBody = _message.RequestBody
            };

            _stepsDescriptions = (from pas in _dbContext.PluginActionSets
                                  join pasi in _dbContext.PluginActionSetItems
                                      on pas.Id equals pasi.PluginActionSetId
                                  join rp in _dbContext.RegisteredPlugins
                                      on pasi.RegisteredPluginId equals rp.Id
                                  join pa in _dbContext.PluginAsseblies
                                        on rp.AssemblyId equals pa.Id
                                  where pas.Id == _pluginActionSetId
                                  orderby pasi.Order
                                  select new PipelineStepDescription
                                  {
                                      AssemblyId = rp.Id,
                                      Class = rp.Class,
                                      Order = pasi.Order,
                                      IsAsynchronous = false,
                                      Modified = pa.Modified,
                                      ConfigurationString = pasi.Configuration,
                                  }).ToList();

            _stepsDescriptions.ForEach(psd =>
            {
                if (!string.IsNullOrEmpty(psd.ConfigurationString))
                {
                    _context.PluginStepSettings = JsonConvert.DeserializeObject<Dictionary<string, object>>(psd.ConfigurationString);
                }
            });

            var globalSettings = _dbContext.GlobalSettings.ToList();

            globalSettings.ForEach(globalSetting =>
            {
                _context.GlobalSettings = _workflowExecutionContext.GlobalSettings;
            });
        }

        public void Run()
        {
            _message.StatusCode = (int)StatusCodeEnum.OnTheAnvil;
            _dbContext.SaveChanges();

            foreach (PipelineStepDescription stepDescription in _stepsDescriptions)
            {
                string assemblyName = AssembliesPreLoader.WarmupAssembly(stepDescription);

                Assembly assembly = Assembly.LoadFrom(assemblyName);
                Type type = assembly.GetType(stepDescription.Class);

                ExecutePlugin(type, stepDescription);
            }

            _message.ResponseBody = _context.ResponseBody;
            _message.ResponseSystem = _context.ResponseSystem;
            _message.ResponseUser = _context.ResponseUser;
            _message.StatusCode = _context.StatusCode ?? (int)StatusCodeEnum.Processed;

            _dbContext.SaveChanges();
        }

        private void ExecutePlugin(Type type, PipelineStepDescription stepDescription)
        {
            try
            {
                IPipelinePlugin plugin = (IPipelinePlugin)Activator.CreateInstance(type, _context);
                plugin.BeforeExecution();
                plugin.Execute();
                plugin.AfterExecution();
            }
            catch (Exception ex)
            {
                _message.ErrorCode = 100000;
                _message.ErrorMessage = ex.Message;

                ProcessingError pe = new ProcessingError
                {
                    MessageId = _messageId,
                    StackTrace = ex.StackTrace,
                    Error = ex.Message
                };

                _dbContext.ProcessingErrors.Add(pe);

                _dbContext.SaveChanges();
            }
        }
    }
}
