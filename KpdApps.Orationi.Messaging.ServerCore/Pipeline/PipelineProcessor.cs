using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KpdApps.Orationi.Messaging.DataAccess;
using KpdApps.Orationi.Messaging.DataAccess.Models;
using KpdApps.Orationi.Messaging.Sdk;
using KpdApps.Orationi.Messaging.Sdk.Plugins;
using log4net;
using Newtonsoft.Json;

namespace KpdApps.Orationi.Messaging.ServerCore.Pipeline
{
    public class PipelineProcessor : IDisposable
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(PipelineProcessor));

        private Guid _workflowId;
        private Guid _messageId;
        private int _requestCode;
        private Guid _pluginActionSetId;

        private IPipelineExecutionContext _pipelineExecutionContext;

        private OrationiDatabaseContext _dbContext;
        private List<PipelineStepDescription> _stepsDescriptions;

        public PipelineProcessor(IPipelineExecutionContext pipelineExecutionContext, Workflow.WorkflowAction workflowAction)
        {
            _pipelineExecutionContext = pipelineExecutionContext;

            _messageId = pipelineExecutionContext.MessageId;
            _requestCode = pipelineExecutionContext.RequestCode;

            _workflowId = workflowAction.WorkflowId;
            _pluginActionSetId = workflowAction.PluginActionSetId;
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
            log.Debug("Инициализация...");
            _dbContext = new OrationiDatabaseContext();

            _stepsDescriptions = (from pas in _dbContext.PluginActionSets
                                  join pasi in _dbContext.PluginActionSetItems on pas.Id equals pasi.PluginActionSetId
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
        }

        public void Run()
        {

            foreach (PipelineStepDescription stepDescription in _stepsDescriptions)
            {
                var workflowExecutionStep = new WorkflowExecutionStep
                {
                    WorkflowId = _workflowId,
                    PluginActionSetId = _pluginActionSetId,
                    StatusCode = (int)PipelineStatusCodes.New,
                    MessageId = (Guid?)_messageId
                };

                _dbContext.WorkflowExecutionSteps.Add(workflowExecutionStep);
                _dbContext.SaveChanges();

                try
                {
                    workflowExecutionStep.StatusCode = (int)PipelineStatusCodes.InProgress;
                    _dbContext.SaveChanges();

                    if (!string.IsNullOrEmpty(stepDescription.ConfigurationString))
                    {
                        log.Debug($"Загрузка конфигурации для {stepDescription.Class}\r\n" +
                                  $"Строка конфигурации - {stepDescription.ConfigurationString}");
                        _pipelineExecutionContext.PluginStepSettings =
                            JsonConvert.DeserializeObject<Dictionary<string, object>>(
                                stepDescription.ConfigurationString);
                    }
                    else
                    {
                        _pipelineExecutionContext.PluginStepSettings = null;
                    }

                    string assemblyName = AssembliesPreLoader.WarmupAssembly(stepDescription);

                    Assembly assembly = Assembly.LoadFrom(assemblyName);
                    Type type = assembly.GetType(stepDescription.Class);

                    ExecutePlugin(type);

                    workflowExecutionStep.RequestBody = _pipelineExecutionContext.RequestBody;
                    workflowExecutionStep.ResponseBody = _pipelineExecutionContext.ResponseBody;
                    workflowExecutionStep.StatusCode = (int) PipelineStatusCodes.Finished;
                    _dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    log.Error($"Exception message:\r\n{ex.Message}");
                    log.Error($"Exception stack trace:\r\n{ex.StackTrace}");
                    if (ex.InnerException != null)
                    {
                        log.Error($"Inner exception message:\r\n{ex.InnerException.Message}");
                        log.Error($"Inner exception stack trace:\r\n{ex.InnerException.StackTrace}");
                    }

                    workflowExecutionStep.StatusCode = (int)PipelineStatusCodes.Error;
                    _dbContext.SaveChanges();
                    throw;
                }
            }
        }

        private void ExecutePlugin(Type type)
        {
            try
            {
                log.Debug($"Инстанцирование типа — {type.FullName}");
                log.Debug($"Рабочий каталог домена приложения: {AppDomain.CurrentDomain.BaseDirectory}");
                IPipelinePlugin plugin = (IPipelinePlugin)Activator.CreateInstance(type, _pipelineExecutionContext);
                log.Debug($"Инстанцирование типа — {type.FullName} завершено!");
                plugin.BeforeExecution();
                plugin.Execute();
                plugin.AfterExecution();
            }
            catch (Exception ex)
            {
                log.Error($"Exception message:\r\n{ex.Message}");
                log.Error($"Exception stack trace:\r\n{ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    log.Error($"Inner exception message:\r\n{ex.InnerException.Message}");
                    log.Error($"Inner exception stack trace:\r\n{ex.InnerException.StackTrace}");
                }
                
                ProcessingError pe = new ProcessingError
                {
                    MessageId = _messageId,
                    StackTrace = ex.StackTrace,
                    Error = ex.Message
                };

                _dbContext.ProcessingErrors.Add(pe);
                _dbContext.SaveChanges();
                throw;
            }
        }
    }
}
