using System;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using KpdApps.Orationi.Messaging.DataAccess;
using KpdApps.Orationi.Messaging.ServerCore.Pipeline;

namespace KpdApps.Orationi.Messaging.ServerCore
{
    public static class AssembliesPreLoader
    {
        private const string AssembliesTempFolderName = "Plugins";
        private static readonly Regex AssembliesNamingTemplate = new Regex("^[0-9A-Fa-f]{8}-[0-9A-Fa-f]{4}-[0-9A-Fa-f]{4}-[0-9A-Fa-f]{4}-[0-9A-Fa-f]{12}-\\d+\\.dll$");
        private static readonly string BasePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static void Execute()
        {
            OrationiDatabaseContext _dbContext = new OrationiDatabaseContext();

            var assemblies = (from pasi in _dbContext.PluginActionSetItems
                              join rp in _dbContext.RegisteredPlugins on pasi.RegisteredPluginId equals rp.Id
                              join pa in _dbContext.PluginAsseblies on rp.AssemblyId equals pa.Id
                              select new
                              {
                                  Id = pa.Id,
                                  AssemblyBinary = pa.Assembly,
                                  Modified = pa.Modified
                              }
                             ).Distinct().ToList();

            string tmpAssembliesPath = Path.Combine(BasePath, AssembliesTempFolderName);

            if (Directory.Exists(tmpAssembliesPath))
            {
                DirectoryInfo di = new DirectoryInfo(tmpAssembliesPath);
                foreach (FileInfo file in di.GetFiles())
                {
                    if (AssembliesNamingTemplate.IsMatch(file.Name))
                    {
                        file.Delete();
                    }
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
            OrationiDatabaseContext _dbContext = new OrationiDatabaseContext();

            var assemblies = (from pa in _dbContext.PluginAsseblies
                              where pa.Id == assemblyId
                              select new
                              {
                                  Id = pa.Id,
                                  AssemblyBinary = pa.Assembly,
                                  Modified = pa.Modified
                              }
                             ).ToList().Distinct();

            string tmpAssembliesPath = Path.Combine(BasePath, AssembliesTempFolderName);
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
            string tmpAssembliesPath = Path.Combine(BasePath, AssembliesTempFolderName);
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
