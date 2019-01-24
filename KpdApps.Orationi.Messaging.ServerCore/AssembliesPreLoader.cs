using System;
using System.Linq;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using KpdApps.Orationi.Messaging.DataAccess;
using KpdApps.Orationi.Messaging.DataAccess.Models;
using KpdApps.Orationi.Messaging.ServerCore.Pipeline;

namespace KpdApps.Orationi.Messaging.ServerCore
{
    public static class AssembliesPreLoader
    {
        private const string AssembliesFolderName = "Plugins";
        private static readonly string BasePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static void Execute()
        {
            using (var dbContext = new OrationiDatabaseContext())
            {
                var assembliesIdList = dbContext
                    .PluginAsseblies
                    .Select(pa => pa.Id)
                    .ToList();

                foreach (var assemblyId in assembliesIdList)
                {
                    Execute(assemblyId);
                }
            }
        }

        /// <summary>
        /// Удаляет все данные из папки cref="AssembliesTempFolderName" и саму папку, а после загружает все актуальные сборки/>
        /// </summary>
        public static void Reload()
        {
            string assembliesPath = Path.Combine(BasePath, AssembliesFolderName);

            if (Directory.Exists(assembliesPath))
            {
                Directory.Delete(assembliesPath, true);
            }

            Execute();
        }

        public static void Execute(Guid assemblyId)
        {
            using (var dbContext = new OrationiDatabaseContext())
            {
                var pluginAssembly = dbContext.PluginAsseblies.FirstOrDefault(pa => pa.Id == assemblyId);
                if (pluginAssembly is null)
                {
                    throw new InvalidOperationException($"Не найдена сборка с Id - {assemblyId}");
                }

                var pluginAssemblyInfo = PluginAssemblyInfo.Create(pluginAssembly);

                if (!pluginAssemblyInfo.IsFolderExists)
                {
                    Directory.CreateDirectory(pluginAssemblyInfo.BaseFolder);
                }

                AppDomain.CurrentDomain.AppendPrivatePath(pluginAssemblyInfo.BaseFolder);

                var zipAssemblyPackage= $"{pluginAssemblyInfo.FullPath}.zip";

                using (var writer = new BinaryWriter(File.OpenWrite(zipAssemblyPackage)))
                {
                    writer.Write(pluginAssembly.Assembly, 0, pluginAssembly.Assembly.Length);
                }

                ZipFile.ExtractToDirectory(zipAssemblyPackage, pluginAssemblyInfo.BaseFolder);
                
                File.Delete(zipAssemblyPackage);
            }
        }
        
        internal static string WarmupAssembly(PipelineStepDescription stepDescription)
        {
            var pluginAssemblyInfo = PluginAssemblyInfo.Create(stepDescription);
            
            if (!pluginAssemblyInfo.IsAssemblyExist)
            {
                Execute(stepDescription.AssemblyId);
            }

            return pluginAssemblyInfo.FullPath;
        }

        private class PluginAssemblyInfo
        {
            public static PluginAssemblyInfo Create(PipelineStepDescription stepDescription)
            {
                long unixTimeSec = ((DateTimeOffset)stepDescription.Modified).ToUnixTimeSeconds();

                var folder = Path.Combine(BasePath,
                    AssembliesFolderName,
                    $"{stepDescription.AssemblyName}-{unixTimeSec}");

                var fullPath = Path.Combine(folder,
                    $"{stepDescription.AssemblyName}.dll");

                return new PluginAssemblyInfo
                {
                    BaseFolder = folder,
                    FullPath = fullPath
                };
            }

            public static PluginAssemblyInfo Create(PluginAssembly assembly)
            {
                long unixTimeSec = ((DateTimeOffset)assembly.Modified).ToUnixTimeSeconds();

                var folder = Path.Combine(BasePath,
                    AssembliesFolderName,
                    $"{assembly.Name}-{unixTimeSec}");

                var fullPath = Path.Combine(folder,
                    $"{assembly.Name}.dll");

                return new PluginAssemblyInfo
                {
                    BaseFolder = folder,
                    FullPath = fullPath
                };
            }

            public string BaseFolder { get; set; }
            public string FullPath { get; set; }

            public bool IsAssemblyExist => File.Exists(FullPath);

            public bool IsFolderExists => Directory.Exists(BaseFolder);
        }
    }
}
