using KpdApps.Orationi.Messaging.DataAccess.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace KpdApps.Orationi.Messaging.DataAccessTests
{
    [TestClass]
    public class PluginAssembliesTests : BaseDataAccessTest
    {
        [TestMethod]
        public void InsertPluginAssembly()
        {
            byte[] bytes;
            string fileName = "KpdApps.Orationi.Messaging.DummyPlugins.dll";
            using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
            {
                bytes = ReadAllBytes(reader);
            }

            PluginAssembly pluginAssembly = new PluginAssembly();
            pluginAssembly.Name = "Unit Test";
            pluginAssembly.Assembly = bytes;

            try
            {
                DbContext.Add(pluginAssembly);
                DbContext.SaveChanges();
                Assert.IsTrue(pluginAssembly.Id != Guid.Empty, "Id Empty Guid");
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            finally
            {
                DbContext.PluginAsseblies.Remove(pluginAssembly);
                DbContext.SaveChanges();
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
