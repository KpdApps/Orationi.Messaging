using KpdApps.Orationi.Messaging.DataAccess;
using KpdApps.Orationi.Messaging.DataAccess.Models;
using KpdApps.Orationi.Messaging.Sdk.Plugins;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace KpdApps.Orationi.Messaging.ServerCore.Pipeline
{
    public class Pipeline : IDisposable
    {
        private Guid _messageId;
        private int _requestCode;
        private IExecuteContext _context;
        private OrationiMessagingContext _dbContext;
        private IEnumerable<PipelineStepDescription> _stepsDescriptions;
        private Message _message;

        public Pipeline(Guid messageId, int requestCode)
        {
            _messageId = messageId;
            _requestCode = requestCode;
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
            //string connectionString = ConfigurationManager.ConnectionStrings[0].ConnectionString;
            DbContextOptionsBuilder<OrationiMessagingContext> optionsBuilder = new DbContextOptionsBuilder<OrationiMessagingContext>();
            optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=OrationiMessageBus;Integrated Security=True");//);

            _dbContext = new OrationiMessagingContext(optionsBuilder.Options);
            _message = _dbContext.Messages.FirstOrDefault(m => m.Id == _messageId);
            _message.AttemptCount++;
            _dbContext.SaveChanges();

            ExecuteContext context = new ExecuteContext
            {
                RequestBody = _message.RequestBody
            };

            _context = context;

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
                                      ModifiedOn = pa.ModifiedOn
                                  }).AsEnumerable();
        }

        public async void Run()
        {
            List<Task> tasks = new List<Task>();

            foreach (PipelineStepDescription stepDescription in _stepsDescriptions)
            {
                string tmpAssembliesPath = Path.Combine(Directory.GetCurrentDirectory(), "tmp");
                long unixTimeSec = ((DateTimeOffset)stepDescription.ModifiedOn).ToUnixTimeSeconds();
                string asseblyName = Path.Combine(tmpAssembliesPath, $"{stepDescription.AssemblyId}-{unixTimeSec}.dll");

                if (!File.Exists(asseblyName))
                {
                    AssembliesPreLoader.Execute(stepDescription.AssemblyId);
                }

                Assembly assembly = Assembly.LoadFrom(asseblyName);
                Type type = assembly.GetType(stepDescription.Class);

                if (stepDescription.IsAsynchronous)
                {
                    IPipelinePlugin plugin = (IPipelinePlugin)Activator.CreateInstance(type, _context);
                    plugin.BeforeExecution();
                    plugin.Execute();
                    plugin.AfterExecution();
                }
                else
                {
                    IPipelinePlugin plugin = (IPipelinePlugin)Activator.CreateInstance(type, _context);
                    tasks.Add(Task.Run(() =>
                    {
                        plugin.BeforeExecution();
                        plugin.Execute();
                        plugin.AfterExecution();
                    }));
                }
            }

            await Task.WhenAll(tasks);

            _message.ResponseBody = _context.ResponseBody;
            _message.ResponseSystem = "test";
            _message.ResponseUser = "test";

            _dbContext.SaveChanges();
        }
    }
}
