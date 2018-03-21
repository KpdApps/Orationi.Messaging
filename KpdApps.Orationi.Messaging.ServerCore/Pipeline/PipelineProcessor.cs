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
using KpdApps.Orationi.Messaging.Sdk;

namespace KpdApps.Orationi.Messaging.ServerCore.Pipeline
{
    public class PipelineProcessor : IDisposable
    {
        private Guid _messageId;
        private int _requestCode;
        private Guid _pluginActionSetId;

        private IPipelineExecutionContext _pipelineExecutionContext;
        private IWorkflowExecutionContext _workflowExecutionContext;

        private OrationiMessagingContext _dbContext;
        private List<PipelineStepDescription> _stepsDescriptions;

        public PipelineProcessor(IWorkflowExecutionContext workflowExecutionContext, Guid pluginActionSetId)
        {
            _messageId = workflowExecutionContext.MessageId;
            _requestCode = workflowExecutionContext.RequestCode;
            _workflowExecutionContext = workflowExecutionContext;
            _pluginActionSetId = pluginActionSetId;
            Init();
        }

        ~PipelineProcessor()
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

            _pipelineExecutionContext = new PipelineExecutionContext(_workflowExecutionContext);

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
                                      AssemblyId = pa.Id,
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
                    _pipelineExecutionContext.PluginStepSettings = JsonConvert.DeserializeObject<Dictionary<string, object>>(psd.ConfigurationString);
                }
            });
        }

        public void Run()
        {
            foreach (PipelineStepDescription stepDescription in _stepsDescriptions)
            {
                string assemblyName = AssembliesPreLoader.WarmupAssembly(stepDescription);

                Assembly assembly = Assembly.LoadFrom(assemblyName);
                Type type = assembly.GetType(stepDescription.Class);

                ExecutePlugin(type, stepDescription);
            }
        }

        private void ExecutePlugin(Type type, PipelineStepDescription stepDescription)
        {
            try
            {
                IPipelinePlugin plugin = (IPipelinePlugin)Activator.CreateInstance(type, _pipelineExecutionContext);
                plugin.BeforeExecution();
                plugin.Execute();
                plugin.AfterExecution();
            }
            catch (Exception ex)
            {
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
