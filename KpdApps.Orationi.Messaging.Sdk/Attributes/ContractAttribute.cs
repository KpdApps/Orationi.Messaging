using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace KpdApps.Orationi.Messaging.Sdk.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ContractAttribute : Attribute
    {
        public ContractAttribute(string uri)
        {
            Uri = uri;
        }

        /// <summary>
        /// Путь к ресурсу содержащему схему Xsd
        /// </summary>
        public string Uri { get; }

        /// <summary>
        /// Получение схемы Xsd из вложенных ресурсов предоставленной сборки
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public string GetXsd(Assembly assembly)
        {
            using (var resourceStream = assembly.GetManifestResourceStream(Uri))
            using (var streamReader = new StreamReader(resourceStream, Encoding.UTF8))
            {
                return streamReader.ReadToEnd();
            }
        }
    }
}
