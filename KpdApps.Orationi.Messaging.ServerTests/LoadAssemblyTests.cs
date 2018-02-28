using KpdApps.Orationi.Messaging.DataAccess;
using KpdApps.Orationi.Messaging.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;

namespace KpdApps.Orationi.Messaging.ServerTests
{
    [TestClass]
    public class LoadAssemblyTests
    {
        [TestMethod]
        public void LoadAssemblyTest()
        {
            byte[] bytes;
            string fileName = "KpdApps.Orationi.Messaging.TelegramPlugins.dll";
            //string fileName = "KpdApps.Orationi.Messaging.DummyPlugins.dll";
            using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
            {
                bytes = ReadAllBytes(reader);
            }

            DbContextOptionsBuilder<OrationiMessagingContext> optionsBuilder = new DbContextOptionsBuilder<OrationiMessagingContext>();
            optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=OrationiMessageBus;Integrated Security=True");

            using (OrationiMessagingContext dbContext = new OrationiMessagingContext(optionsBuilder.Options))
            {
                PluginAssembly pa = dbContext.PluginAsseblies.FirstOrDefault(p => p.Name == fileName);
                if (pa == null)
                {
                    pa = new PluginAssembly();
                    dbContext.Add(pa);
                }
                pa.Name = fileName;
                pa.Assembly = bytes;
                dbContext.SaveChanges();
            }
        }

        public byte[] ReadAllBytes(BinaryReader reader)
        {
            const int bufferSize = 4096;
            using (var ms = new MemoryStream())
            {
                byte[] buffer = new byte[bufferSize];
                int count;
                while ((count = reader.Read(buffer, 0, buffer.Length)) != 0)
                    ms.Write(buffer, 0, count);
                return ms.ToArray();
            }
        }
    }
}