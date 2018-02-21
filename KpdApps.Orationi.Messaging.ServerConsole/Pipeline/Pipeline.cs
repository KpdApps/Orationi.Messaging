using KpdApps.Orationi.Messaging.DataAccess;
using KpdApps.Orationi.Messaging.DataAccess.Models;
using KpdApps.Orationi.Messaging.Sdk.Plugins;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;

namespace KpdApps.Orationi.Messaging.ServerConsole.Pipeline
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
            //ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString
            DbContextOptionsBuilder<OrationiMessagingContext> optionsBuilder = new DbContextOptionsBuilder<OrationiMessagingContext>();
            optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=OrationiMessageBus;Integrated Security=True");

            _dbContext = new OrationiMessagingContext(optionsBuilder.Options);
            _message = _dbContext.Messages.FirstOrDefault(m => m.Id == _messageId);

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
                                      Assembly = pa.Assembly,
                                      Class = pt.Class,
                                      Order = prs.Order,
                                      IsAsynchronous = prs.IsAsynchronous
                                  }).AsEnumerable();
        }

        public void Run()
        {
            foreach (PipelineStepDescription stepDescription in _stepsDescriptions)
            {
                string fileName = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid().ToString()}.dll");

                using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(fileName)))
                {
                    writer.Write(stepDescription.Assembly, 0, stepDescription.Assembly.Length);
                }

                Assembly assembly = Assembly.LoadFrom(fileName);
                AppDomain.CurrentDomain.Load(stepDescription.Assembly);
                Type type = assembly.GetType(stepDescription.Class);
                IPipelinePlugin plugin = (IPipelinePlugin)Activator.CreateInstance(type, _context);
                plugin.BeforeExecution();
                plugin.Execute();
                plugin.AfterExecution();
                File.Delete(fileName);
            }

            _message.ResponseBody = _context.ResponseBody;
            _message.ResponseSystem = "test";
            _message.ResponseUser = "test";

            _dbContext.SaveChanges();
        }
    }
}
