using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
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

        [JsonProperty("RequsetCode")]
        public int RequsetCode { get; set; }

        private const int MaxSharePointFileNameLegth = 250;

        public static void ValidateFileName(string fileName, HttpRequestMessage request)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new HttpResponseException(request.CreateResponse(
                    HttpStatusCode.BadRequest,
                    new Response { IsError = true, Error = "Передайте не пустое имя файла" })
                );

            if (fileName.Length > MaxSharePointFileNameLegth)
                throw new HttpResponseException(request.CreateResponse(
                    HttpStatusCode.BadRequest,
                    new Response { IsError = true, Error = $"Имя файла слишком длинное, максимум {MaxSharePointFileNameLegth} символов" })
                );

            string[] forbidenExtensions = { ".exe", ".dll" };

            var fileExtension = Path.GetExtension(fileName);

            if (forbidenExtensions.Contains(fileExtension.ToLower()))
                throw new HttpResponseException(request.CreateResponse(
                    HttpStatusCode.BadRequest,
                    new Response { IsError = true, Error = $"Нельзя загружать файлы с расширением {fileExtension}" })
                );
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
    }
}