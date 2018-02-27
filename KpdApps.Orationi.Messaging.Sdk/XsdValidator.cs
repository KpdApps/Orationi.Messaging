using System;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Schema;

namespace KpdApps.Orationi.Messaging.Sdk
{
    public class XsdValidator
    {
        public static void ValidateXml(string xml, string[] xsdFilePath, Type schemaType)
        {
            var xdoc = XDocument.Parse(xml);
            var schemas = new XmlSchemaSet();
            Assembly assembly = schemaType.Assembly;
            foreach (var xsdPath in xsdFilePath)
            {
                using (var schemaStream = assembly.GetManifestResourceStream(xsdPath))
                {
                    if (schemaStream == null)
                    {
                        throw new Exception("XML Schema Definition not found.");
                    }

                    using (var schemaReader = System.Xml.XmlReader.Create(schemaStream))
                    {
                        schemas.Add(null, schemaReader);
                    }
                }
            }
            xdoc.Validate(schemas, (sender1, args) =>
            {
                if (args.Severity == XmlSeverityType.Error)
                {
                    throw new Exception(String.Format("XML Schema Definition validation error: {0}", args.Exception.Message));
                }
            });
        }
    }
}
