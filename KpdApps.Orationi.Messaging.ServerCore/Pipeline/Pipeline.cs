using KpdApps.Orationi.Messaging.DataAccess;
using KpdApps.Orationi.Messaging.DataAccess.Models;
using KpdApps.Orationi.Messaging.Sdk.Plugins;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using KpdApps.Orationi.Messaging.ServerCore.Helpers;

namespace KpdApps.Orationi.Messaging.ServerCore.Pipeline
{
    public class Pipeline : IDisposable
    {
        private Guid _messageId;
        private int _requestCode;
        private IExecuteContext _context;
        private OrationiMessagingContext _dbContext;
        private List<PipelineStepDescription> _stepsDescriptions;
        private Message _message;

        public Pipeline(Guid messageId, int requestCode)
        {
            _messageId = messageId;
            _requestCode = requestCode;
            Init();
        }

        ~Pipeline()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_dbContext is null)
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

            _stepsDescriptions = (from prs in _dbContext.PluginRegisteredSteps
                                  join pt in _dbContext.PluginTypes on prs.PluginTypeId equals pt.Id
                                  join pa in _dbContext.PluginAsseblies on pt.AssemblyId equals pa.Id
                                  where prs.RequestCode == _requestCode
                                  orderby prs.Order
                                  select new PipelineStepDescription
                                  {
                                      AssemblyId = pa.Id,
                                      Class = pt.Class,
                                      Order = prs.Order,
                                      IsAsynchronous = prs.IsAsynchronous,
                                      Modified = pa.Modified,
                                      ConfigurationString = prs.Configuration,

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
		        _context.GlobalSettings.Add(globalSetting.Name, globalSetting.Value);
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
