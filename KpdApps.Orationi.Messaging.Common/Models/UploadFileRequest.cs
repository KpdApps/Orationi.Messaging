using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace KpdApps.Orationi.Messaging.Common.Models
{
    public class UploadFileRequest
    {
        [JsonProperty("ObjectId")]
        public Guid ObjectId { get; set; }

        [JsonProperty("ObjectCode")]
        public int ObjectCode { get; set; }

        [JsonProperty("FileType")]
        public string FileType { get; set; }

        [JsonProperty("RequestCode")]
        public int RequestCode { get; set; }

        private const int MaxSharePointFileNameLegth = 250;

        public static void ValidateFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new InvalidOperationException("Передайте не пустое имя файла");
            }

            if (fileName.Length > MaxSharePointFileNameLegth)
            {
                throw new InvalidOperationException($"Имя файла слишком длинное, максимум {MaxSharePointFileNameLegth} символов");

            }

            string[] forbidenExtensions = { ".exe", ".dll" };

            var fileExtension = Path.GetExtension(fileName);

            if (forbidenExtensions.Contains(fileExtension.ToLower()))
            {
                throw new InvalidOperationException($"Нельзя загружать файлы с расширением {fileExtension}");
            }
        }

        public string ToXmlString()
        {
            var xmlBody = new XElement("UploadFileRequest",
                new XElement("ObjectId", ObjectId),
                new XElement("ObjectCode", ObjectCode),
                new XElement("FileType", FileType)
            );

            return @"<?xml version=""1.0"" encoding=""utf-8""?>" + xmlBody.ToString(SaveOptions.DisableFormatting);
        }

        public override string ToString()
        {
            return $"\"ObjectId\" : \"{ObjectId}\",\r\n\"ObjectCode\" : \"{ObjectCode}\",\r\n\"FileType\" : \"{FileType}\",\r\n\"RequestCode\" : \"{RequestCode}\"";
        }
    }
}