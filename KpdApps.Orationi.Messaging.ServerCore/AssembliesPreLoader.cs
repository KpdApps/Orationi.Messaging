using KpdApps.Orationi.Messaging.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using KpdApps.Orationi.Messaging.ServerCore.Pipeline;

namespace KpdApps.Orationi.Messaging.ServerCore
{
    public static class AssembliesPreLoader
    {
        const string AssembliesTempFolderName = "tmp";

        public static void Execute()
        {
            OrationiMessagingContext _dbContext = new OrationiMessagingContext(OrationiMessagingContextExtension.DefaultDbContextOptions());

            var assemblies = (from prs in _dbContext.PluginRegisteredSteps
                              join pt in _dbContext.PluginTypes on prs.PluginTypeId equals pt.Id
                              join pa in _dbContext.PluginAsseblies on pt.AssemblyId equals pa.Id
                              select new
                              {
                                  Id = pa.Id,
                                  AssemblyBinary = pa.Assembly,
                                  Modified = pa.Modified
                              }
                             ).ToList().Distinct();

            string tmpAssembliesPath = Path.Combine(Directory.GetCurrentDirectory(), "tmp");

            if (Directory.Exists(tmpAssembliesPath))
            {
                DirectoryInfo di = new DirectoryInfo(tmpAssembliesPath);
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
            }
            else
            {
                Directory.CreateDirectory(tmpAssembliesPath);
            }

            foreach (var assembly in assemblies)
            {
                long unixTimeSec = ((DateTimeOffset)assembly.Modified).ToUnixTimeSeconds();
                string asseblyName = Path.Combine(tmpAssembliesPath, $"{assembly.Id}-{unixTimeSec}.dll");
                using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(asseblyName)))
                {
                    writer.Write(assembly.AssemblyBinary, 0, assembly.AssemblyBinary.Length);
                }
            }
        }

        public static void Execute(Guid assemblyId)
        {
            OrationiMessagingContext _dbContext = new OrationiMessagingContext(OrationiMessagingContextExtension.DefaultDbContextOptions());

            var assemblies = (from pa in _dbContext.PluginAsseblies
                              where pa.Id == assemblyId
                              select new
                              {
                                  Id = pa.Id,
                                  AssemblyBinary = pa.Assembly,
                                  Modified = pa.Modified
                              }
                             ).ToList().Distinct();

            string tmpAssembliesPath = Path.Combine(Directory.GetCurrentDirectory(), "tmp");
            Directory.CreateDirectory(tmpAssembliesPath);
            foreach (var assembly in assemblies)
            {
                long unixTimeSec = ((DateTimeOffset)assembly.Modified).ToUnixTimeSeconds();
                string asseblyName = Path.Combine(tmpAssembliesPath, $"{assembly.Id}-{unixTimeSec}.dll");
                using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(asseblyName)))
                {
                    writer.Write(assembly.AssemblyBinary, 0, assembly.AssemblyBinary.Length);
                }
            }
        }

        internal static string WarmupAssembly(PipelineStepDescription stepDescription)
        {
            string tmpAssembliesPath = Path.Combine(Directory.GetCurrentDirectory(), AssembliesTempFolderName);
            long unixTimeSec = ((DateTimeOffset)stepDescription.Modified).ToUnixTimeSeconds();
            string assemblyName = Path.Combine(tmpAssembliesPath, $"{stepDescription.AssemblyId}-{unixTimeSec}.dll");

            if (!File.Exists(assemblyName))
            {
                Execute(stepDescription.AssemblyId);
            }

            return assemblyName;
        }
    }
}
